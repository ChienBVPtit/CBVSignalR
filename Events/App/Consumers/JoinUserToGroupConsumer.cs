using CBVSignalR.Application.Base.Event;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Context;
using CBVSignalR.Events.App.Interfaces;
using CBVSignalR.Events.App.Models;
using CBVSignalR.Events.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CBVSignalR.Events.App.Consumers
{
    public class JoinUserToGroupConsumer : BaseConsumer<JoinUserToGroupEvent>
    {
        protected override string QueueName => "join-user-to-groupsubscription-queue";
        protected override string RoutingKey => "joinusertogroup";

        private readonly ILogger<JoinUserToGroupConsumer> _logger;

        public JoinUserToGroupConsumer(
        ILogger<JoinUserToGroupConsumer> logger,
        IRabbitMqConnection connection,
        IServiceScopeFactory scopeFactory)
        : base(connection, scopeFactory)
        {
            _logger = logger;
        }

        protected override async Task HandleAsync(
        JoinUserToGroupEvent @event,
        IServiceProvider serviceProvider)
        {
            _logger.LogInformation(
            $"📥 Received JoinUserToGroupEvent | EventId={@event.EventId}",
            @event.EventId);

            var handler =
                serviceProvider.GetRequiredService<IJoinUserToGroupHandler>();

            await handler.HandleAsync(@event);
        }
    }
}
