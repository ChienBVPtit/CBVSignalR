using CBVSignalR.Events.App.Consumers;

namespace CBVSignalR.Events.App.Runners
{
    public class JoinUserToGroupConsumerHostedService : BackgroundService
    {
        private readonly JoinUserToGroupConsumer _consumer;

        public JoinUserToGroupConsumerHostedService(
            JoinUserToGroupConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Start();
            return Task.CompletedTask;
        }
    }
}
