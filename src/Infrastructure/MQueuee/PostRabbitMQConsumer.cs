using System;
using System.Threading;
using System.Threading.Tasks;
using Boyner.CaseStudy.ApplicationCore.Commands;
using Boyner.CaseStudy.ApplicationCore.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Boyner.CaseStudy.Infrastructure.RabbitMQ
{
    public class PostRabbitMQConsumer : BackgroundService
    {
        private readonly ILogger<PostRabbitMQConsumer> _logger;
        public IServiceProvider Services { get; }
        private readonly IRabbitMQ<Post> _r;
        private readonly IMediator _mediator;
        public PostRabbitMQConsumer(ILogger<PostRabbitMQConsumer> logger, IServiceProvider services, IRabbitMQ<Post> r, IMediator mediator)
        {

            _logger = logger;
            Services = services;
            _r = r;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);

        }

        private Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

           var scope = Services.CreateScope();
            // using (var scope = Services.CreateScope())
            // {
                var scopedProcessingService =
                    scope.ServiceProvider.GetRequiredService<IRabbitMQ<Post>>();
                var mediator =
                    scope.ServiceProvider.GetRequiredService<IMediator>();

                scopedProcessingService.StartConsume(async (model) =>
              {
                  await mediator.Send(new SavePostCommand(model.Id, model.Text));
              });

            // }

        //     _r.StartConsume(async (model) =>
        //   {
        //       await _mediator.Send(new SavePostCommand(model.Id, model.Text));
        //   });


            return Task.CompletedTask;
        }
    }
}