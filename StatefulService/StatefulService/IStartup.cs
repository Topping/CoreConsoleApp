using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StatefulService.StatefulService
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}