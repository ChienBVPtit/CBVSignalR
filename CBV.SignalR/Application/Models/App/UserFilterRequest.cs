namespace CBVSignalR.Application.Models.App
{
    public class UserFilterRequest : PagingFilterRequest
    {
        // Tên đầy đủ của user
        public string FullName { get; set; } = null!;
        // Username của user
        public string UserName { get; set; } = null!;
        // Id của user bên phía Identity
        public string UserId { get; set; } = null!;
        // Email của user
        public string? Email { get; set; } = null!;
        // Số điện thoại của user
        public string? PhoneNumber { get; set; } = null!;
    }
}
