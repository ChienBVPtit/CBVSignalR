using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;

namespace CBVSignalR.Application.Interfaces
{
    public interface IGroupSubscriptionService : IBaseService<GroupSubscription, Guid>
    {
        Task<GroupSubscription> GetGroupSubscriptionByName(string name);
    }
}
