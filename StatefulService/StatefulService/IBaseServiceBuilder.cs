using System;
using Microsoft.Extensions.DependencyInjection;

namespace StatefulService.StatefulService
{
    public interface IBaseServiceBuilder
    {
        IBaseServiceBuilder ConfigureServices(Action<IServiceCollection> configureServices);
        IBaseServiceBuilder ForRunnableService<TRunnable>() where TRunnable : IRunnableService;
        IRunnableService Build();
    }
}