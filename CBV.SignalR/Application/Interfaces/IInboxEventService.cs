using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Models.App;

namespace CBVSignalR.Application.Interfaces
{
    public interface IInboxEventService : IBaseService<InboxEvent, Guid, InboxEventFilterRequest>
    {
    }
}
