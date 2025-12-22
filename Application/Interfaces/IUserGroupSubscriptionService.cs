using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Models.App;

namespace CBVSignalR.Application.Interfaces
{
    public interface IUserGroupSubscriptionService : IBaseService<UserGroupSubscription, Guid, UserGroupSubscriptionFilterRequest>
    {
        Task<bool> IsUserGroupExis(string userId, Guid groupId);
    }
}
