﻿using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Dependency.Injection.Extensions.Decorator
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureDependencyInjection(services);

            var serviceProvider = services.BuildServiceProvider();
            Run(serviceProvider);
        }

        static void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<TextWriter>(Console.Out);

            bool useOption1 = true;

            if (useOption1)
            {
                services.AddSingleton<ISampleService>(
                        Decorate<ISampleService>
                            .This<SampleService>()
                            .With<LoggingSampleService>()
                            .Factory()
                    );
            }
            else
            {
                services.AddSingleton<SampleService>();
                services.AddSingleton<ISampleService>(Decorate.WithInnerType<LoggingSampleService, SampleService>());
            }
        }

        static void Run(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetRequiredService<ISampleService>();

            service.Process(5);
        }
    }
}
