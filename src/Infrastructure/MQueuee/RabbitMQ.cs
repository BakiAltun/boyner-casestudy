using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Boyner.CaseStudy.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Boyner.CaseStudy.Infrastructure.RabbitMQ
{
    public class RabbitMQ<T> : IRabbitMQ<T> where T : BaseEntity
    {
        const string BROKER_NAME = "";
        const string _queueName = "boyner-case-study-post";
        private string _routingKey;
        private readonly ILogger<RabbitMQ<T>> _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        public RabbitMQ(ILogger<RabbitMQ<T>> logger, IRabbitMQPersistentConnection persistentConnection)
        {
            _logger = logger;
            _persistentConnection = persistentConnection;
            _routingKey = typeof(T).Name;
        }

        public void Publish(T model)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @model.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueDeclare(queue: _routingKey,
                                              durable: true,
                                              exclusive: false,
                                              autoDelete: false,
                                              arguments: null);

                // channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

                var body = JsonSerializer.SerializeToUtf8Bytes(@model, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
              
                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @model.Id);

                    channel.BasicPublish(
                        exchange: BROKER_NAME,
                        routingKey: _routingKey,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }


        private IModel _consumerChannel;
        public void StartConsume(Action<T> process)
        {
            if (_consumerChannel == null)
                _consumerChannel = CreateConsumerChannel(process);

            StartBasicConsume(process);
        }

        private IModel CreateConsumerChannel(Action<T> process)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            // channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

            channel.QueueDeclare(queue: _routingKey,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel(process);
                StartBasicConsume(process);
            };

            return channel;
        }

        private void StartBasicConsume(Action<T> process)
        {

            if (_consumerChannel == null) return;
            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

            consumer.Received += (sender, eventArgs) =>
            {
                var eventName = eventArgs.RoutingKey;
                var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
                var model = JsonSerializer.Deserialize<T>(message, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                process.Invoke(model);

                return Task.CompletedTask;
            };

            _consumerChannel.BasicConsume(
                queue: _routingKey,
                autoAck: false,
                consumer: consumer);

        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }
    }

    public interface IRabbitMQ<T>
    {
        void Publish(T model);
        void StartConsume(Action<T> process);
    }
}