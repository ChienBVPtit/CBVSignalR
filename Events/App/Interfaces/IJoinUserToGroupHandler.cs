using CBVSignalR.Events.App.Models;

namespace CBVSignalR.Events.App.Interfaces
{
    public interface IJoinUserToGroupHandler
    {
        Task HandleAsync(JoinUserToGroupEvent @event);
    }
}
