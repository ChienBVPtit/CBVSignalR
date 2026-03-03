namespace CBVSignalR.Application.Models.App
{
    public class UserNotificationFilterRequest : PagingFilterRequest
    {
        //Id của người dùng
        public string UserId { get; set; } = null!;
        //Id của thông báo
        public string NotificationId { get; set; } = null!;
        //Thông báo đã được đọc chưa
        public bool IsRead { get; set; }
        //Thời gian đọc thông báo
        public DateTime? ReadAt { get; set; }
    }
}
