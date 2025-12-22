namespace CBVSignalR.Application.Models.App
{
    public class NotificationFilterRequest : PagingFilterRequest
    {
        //Id của người dùng
        public string? UserId { get; set; } = null!;
        //Tiêu đề thông báo
        public string? Title { get; set; } = null!;
        //Nội dung thông báo
        public string? Content { get; set; } = null!;
        //Loại thông báo: Thông tin/ Cảnh báo/ Báo cáo/ Hệ thống,...
        public string? Type { get; set; }
        //Thông báo đã được đọc chưa
        public bool? IsRead { get; set; }
        //Thời gian đọc thông báo
        public DateTime? ReadAt { get; set; }
    }
}
