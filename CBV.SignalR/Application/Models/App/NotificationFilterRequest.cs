namespace CBVSignalR.Application.Models.App
{
    public class NotificationFilterRequest : PagingFilterRequest
    {
        //Tiêu đề thông báo
        public string? Title { get; set; } = null!;
        //Nội dung thông báo
        public string? Content { get; set; } = null!;
        //Loại thông báo: Thông tin/ Cảnh báo/ Báo cáo/ Hệ thống,...
        public string? Type { get; set; }
    }
}
