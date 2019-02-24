using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SampleApp.Options;
using StatefulService.StatefulService;

namespace SampleApp
{
    public class App : IRunnableService
    {
        private readonly IOptions<FooOptions> _options;

        public App(IOptions<FooOptions> options)
        {
            _options = options;
        }
        
        public Task Run()
        {
            Console.WriteLine("Started");
            Console.WriteLine($"{_options.Value.Ip}:{_options.Value.Port}");
            return Task.CompletedTask;
        }
    }
}