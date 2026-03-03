namespace CBVSignalR.Application.Models.App
{
    public class GroupSubscriptionFilterRequest : PagingFilterRequest
    {
        //Tên Group
        public string? Name { get; set; } = null!;
        //Mã Group
        public string? Code { get; set; } = null!;
        //Loại Group: group hệ thống - group người dùng,...
        public string? Type { get; set; } = null!;
        //Group có đang hoạt động không
        public bool? IsActive { get; set; }
    }
}
