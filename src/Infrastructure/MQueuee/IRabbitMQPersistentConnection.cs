

using System;
using RabbitMQ.Client;

namespace Boyner.CaseStudy.Infrastructure.RabbitMQ
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}