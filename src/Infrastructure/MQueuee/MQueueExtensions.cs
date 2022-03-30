using Boyner.CaseStudy.ApplicationCore.Entities;
using Boyner.CaseStudy.ApplicationCore.Validations;
using Boyner.CaseStudy.Infrastructure.Data;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using RabbitMQ.Client;
using System.Reflection;

namespace Boyner.CaseStudy.Infrastructure.RabbitMQ
{
    public static class MQueueExtennsions
    {

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterRabbitMQ(configuration);
            services.AddSingleton<IRabbitMQ<Post>, RabbitMQ<Post>>();

            return services;
        }


        private static void RegisterRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                   {
                       var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                       string uri = configuration.GetSection("RabbitMQ").GetValue<string>("Uri");
                       var factory = new ConnectionFactory()
                       {
                           Uri = new System.Uri(uri),
                           DispatchConsumersAsync = true
                       };
 
                       return new DefaultRabbitMQPersistentConnection(factory, logger);
                   });
        }
    }
}