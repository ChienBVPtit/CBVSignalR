using CBVSignalR.Events.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CBVSignalR.Application.Base.Event
{
    public abstract class BaseConsumer<TEvent>
    {
        protected abstract string ExchangeName { get; }
        protected abstract string ExchangeTypeName { get; }
        protected abstract string QueueName { get; }
        protected abstract string RoutingKey { get; }
        protected virtual bool Durable => true;
        protected virtual ushort PrefetchCount => 10;

        private readonly IRabbitMqConnection _connection;
        private readonly IServiceScopeFactory _scopeFactory;

        protected BaseConsumer(
            IRabbitMqConnection connection,
            IServiceScopeFactory scopeFactory)
        {
            _connection = connection;
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            var connection = _connection.GetConnection();
            var channel = connection.CreateModel();
            DeclareTopology(channel);

            // PREFETCH (quan trọng cho scale)
            channel.BasicQos(0, 10, false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var @event = JsonSerializer.Deserialize<TEvent>(body);

                    using var scope = _scopeFactory.CreateScope();
                    await HandleAsync(@event!, scope.ServiceProvider);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch
                {
                    // retry mặc định
                    channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                }
            };

            channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer);
        }

        protected virtual void DeclareTopology(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeTypeName,
                durable: Durable);

            channel.QueueDeclare(
                queue: QueueName,
                durable: Durable,
                exclusive: false,
                autoDelete: false);

            channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: RoutingKey);
        }

        protected abstract Task HandleAsync(
            TEvent @event,
            IServiceProvider serviceProvider);
    }
}
