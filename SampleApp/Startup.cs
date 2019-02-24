using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Options;
using StatefulService.StatefulService;

namespace SampleApp
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FooOptions>(configuration.GetSection("Foo"));
        }
    }
}