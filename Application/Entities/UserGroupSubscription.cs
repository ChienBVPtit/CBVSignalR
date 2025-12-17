using CBVSignalR.Application.Base.Entity;

namespace CBVSignalR.Application.Entities
{
    public class UserGroupSubscription : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public Guid GroupId { get; set; } 
        public GroupSubscription GroupSubscription { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}
