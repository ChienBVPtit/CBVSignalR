using CBVSignalR.Application.Base.Entity;

namespace CBVSignalR.Application.Entities
{
    public class UserNotification : BaseEntity
    {
        //Id của người dùng
        public Guid UserId { get; set; }
        //Id của thông báo
        public Guid NotificationId { get; set; }
        //Thông báo đã được đọc chưa
        public bool IsRead { get; set; }
        //Thời gian đọc thông báo
        public DateTime? ReadAt { get; set; }
        public User User { get; set; } = null!;
        public Notification Notification { get; set; } = null!;
    }
}
