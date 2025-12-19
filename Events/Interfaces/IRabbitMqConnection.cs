using RabbitMQ.Client;

namespace CBVSignalR.Events.Interfaces
{
    public interface IRabbitMqConnection
    {
        IConnection GetConnection();
    }
}
