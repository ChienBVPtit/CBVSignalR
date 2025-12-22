using CBVSignalR.Application.Entities;

namespace CBVSignalR.Application.Models.App
{
    public class UserGroupSubscriptionFilterRequest : PagingFilterRequest
    {
        public string? UserId { get; set; } = null!;
        public Guid? GroupId { get; set; }
        public bool? IsActive { get; set; }
    }
}
