using CBVSignalR.Events.Interfaces;
using RabbitMQ.Client;

namespace CBVSignalR.Events.Connections
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqConnection(IConfiguration config)
        {
            _factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:Username"],
                Password = config["RabbitMQ:Password"],
                VirtualHost = config["RabbitMQ:VirtualHost"],
                Port = 5672, // 5671 nếu dùng TLS/SSL
                DispatchConsumersAsync = true
            };
        }

        public IConnection GetConnection()
         => _factory.CreateConnection();
    }
}
