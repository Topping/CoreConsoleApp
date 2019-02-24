using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StatefulService.StatefulService
{
    public class BaseServiceBuilder : IBaseServiceBuilder
    {
        public static BaseServiceBuilder CreateDefaultServiceBuilder()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(env))
            {
                throw new ArgumentNullException($"ASPNETCORE_ENVIRONMENT variable was not set");
            }

            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(configuration);
            return new BaseServiceBuilder(serviceCollection);
        }

        private bool _serviceBuilt = false;
        private readonly List<Action<IServiceCollection>> _configureServiceActions;
        private readonly IServiceCollection _serviceCollection;
        private Type _runnableType;

        private BaseServiceBuilder(IServiceCollection serviceCollection)
        {
            _configureServiceActions = new List<Action<IServiceCollection>>();
            _serviceCollection = serviceCollection;
        }

        public IBaseServiceBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            _configureServiceActions.Add(configureServices);
            return (IBaseServiceBuilder) this;
        }

        public IBaseServiceBuilder ForRunnableService<TRunnable>() where TRunnable : IRunnableService
        {
            _runnableType = typeof(TRunnable);
            return this;
        }

        public IRunnableService Build()
        {
            if (_serviceBuilt)
            {
                throw new InvalidOperationException("Service has already been built. Can only build a single instance");
            }
            _serviceBuilt = true;
            foreach (var configureServiceAction in _configureServiceActions)
            {
                configureServiceAction(_serviceCollection);
            }

            var serviceProvider = AddServicesFromStartup(_serviceCollection);
            
            try
            {
                return CreateRunnable(in serviceProvider);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private IServiceProvider AddServicesFromStartup(IServiceCollection serviceCollection)
        {
            using (var provider = serviceCollection.BuildServiceProvider())
            {
                var startup = provider.GetService<IStartup>();
                var config = provider.GetService<IConfiguration>();
                startup.ConfigureServices(serviceCollection, config);
                return serviceCollection.BuildServiceProvider();
            }
        }

        private IRunnableService CreateRunnable(in IServiceProvider provider)
        {
            if(_runnableType == null)
                throw new ArgumentNullException($"A runnable type was not specified. Use {nameof(ForRunnableService)} to specify a runnable class");

            var dependencies = new List<dynamic>();
            var runnableConstructor = _runnableType.GetConstructors().FirstOrDefault(c => c.GetParameters().Length != 0);
            if(runnableConstructor == null)
                return Activator.CreateInstance(_runnableType) as IRunnableService;
            
            foreach (var parameter in runnableConstructor.GetParameters())
            {
                var dependency =  provider.GetService(parameter.ParameterType);
                if (dependency != null)
                    dependencies.Add( dependency );
            }

            return (IRunnableService) Activator.CreateInstance(_runnableType, dependencies.ToArray());
        }
    }
}