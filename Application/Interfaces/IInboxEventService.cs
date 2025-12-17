using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;

namespace CBVSignalR.Application.Interfaces
{
    public interface IInboxEventService : IBaseService<InboxEvent, Guid>
    {
    }
}
