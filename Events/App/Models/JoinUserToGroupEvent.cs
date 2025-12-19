using CBVSignalR.Application.Base.Event;

namespace CBVSignalR.Events.App.Models
{
    public class JoinUserToGroupEvent : IntegrationEvent
    {
        public string UserId { get; set; } = null!;
        public Guid? GroupId { get; set; }
        public string GroupName { get; set; } = null!;

    }
}
