using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Boyner.CaseStudy.Infrastructure.Data
{
    public class RabbitMQ
    {
        const string BROKER_NAME = "boyner-case-study";
        const string _queueName = "boyner-case-study-post";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;

        public RabbitMQ()
        {

        }

        public void Publish()
        {

        }

        public void StartConsume<T>(string eventName, Action<T> process)
        {
            if (_consumerChannel == null)
                _consumerChannel = CreateConsumerChannel<T>(process);

            StartBasicConsume<T>(process); 
        }

        private IModel CreateConsumerChannel<T>(Action<T> process)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }


            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel<T>(process);
                StartBasicConsume<T>(process);
            };

            return channel;
        }

        private void StartBasicConsume<T>(Action<T> process)
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
                queue: _queueName,
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
}