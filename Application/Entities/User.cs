using CBVSignalR.Application.Base.Entity;

namespace CBVSignalR.Application.Entities
{
    public class User : BaseEntity
    {
        // Tên đầy đủ của user
        public string FullName { get; set; } = null!;
        // Username của user
        public string UserName { get; set; } = null!;
        // Id của user bên phía Identity
        public Guid UserId { get; set; }
        // Email của user
        public string? Email { get; set; } = null!;
        // Số điện thoại của user
        public string? PhoneNumber { get; set; } = null!;
    }
}
