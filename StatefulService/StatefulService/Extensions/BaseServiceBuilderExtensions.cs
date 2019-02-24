using Microsoft.Extensions.DependencyInjection;

namespace StatefulService.StatefulService.Extensions
{
    public static class BaseServiceBuilderExtensions
    {
        public static IBaseServiceBuilder UseStartup<T>(this IBaseServiceBuilder builder) where T : class, IStartup
        {
            return builder.ConfigureServices(services =>
            {
                ServiceCollectionServiceExtensions.AddSingleton(services, typeof(IStartup), typeof(T));
            });
        }
    }
}