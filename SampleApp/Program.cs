using System;
using System.Threading.Tasks;
using StatefulService.StatefulService;
using StatefulService.StatefulService.Extensions;

namespace SampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateDefaultService.Build().Run();
        }
        
        private static IBaseServiceBuilder CreateDefaultService =>
            BaseServiceBuilder.CreateDefaultServiceBuilder()
                .ForRunnableService<App>()
                .UseStartup<Startup>();
    }
}