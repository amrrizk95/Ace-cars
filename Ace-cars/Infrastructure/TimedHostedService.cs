using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ace_cars.Controllers;
using Ace_cars.Helppers;
using Ace_cars.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Ace_cars.Infrastructure
{
    internal class TimedHostedService : IHostedService
    {
        private readonly ILogger _logger;

        private Timer _timer;


        public TimedHostedService(IServiceProvider services,
            ILogger<TimedHostedService> logger
            )
        {
            Services = services;
            _logger = logger;

        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(2));

            return Task.CompletedTask;

        }

        private void DoWork(object state)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<ProductsController>();

                scopedProcessingService.process();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            return Task.CompletedTask;
        }


    }

}
