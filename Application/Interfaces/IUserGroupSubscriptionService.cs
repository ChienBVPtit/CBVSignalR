using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;

namespace CBVSignalR.Application.Interfaces
{
    public interface IUserGroupSubscriptionService : IBaseService<UserGroupSubscription, Guid>
    {
        Task<bool> IsUserGroupExis(string userId, Guid groupId);
    }
}
