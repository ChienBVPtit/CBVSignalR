using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Models.App;

namespace CBVSignalR.Application.Interfaces
{
    public interface IUserNotificationService : IBaseService<UserNotification, Guid, UserNotificationFilterRequest>
    {
    }
}
