using CBVSignalR.Application.Base.Entity;

namespace CBVSignalR.Application.Entities
{
    public class GroupSubscription : BaseEntity
    {
        //Tên Group
        public string Name { get; set; } = null!;
        //Mã Group
        public string Code { get; set; } = null!;
        //Loại Group: group hệ thống - group người dùng,...
        public string Type { get; set; } = null!;
        //Group có đang hoạt động không
        public bool IsActive { get; set; }
        //Mô tả group
        public string? Description { get; set; }
    }
}
