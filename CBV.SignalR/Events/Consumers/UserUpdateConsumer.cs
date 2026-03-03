using CBV.Shared.Events;
using MassTransit;

namespace CBVSignalR.Events.Consumers
{
    public class UserUpdateConsumer : IConsumer<IUserUpdateEvent>
    {
        private readonly ILogger<UserUpdateConsumer> _logger;
        // Giả sử bạn có DbContext để lưu thông tin user cục bộ
        // private readonly SignalRDbContext _db; 

        public UserUpdateConsumer(ILogger<UserUpdateConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IUserUpdateEvent> context)
        {
            var data = context.Message;

            _logger.LogInformation(">>> Nhận Event cập nhật User: {Id} - {Name}", data.UserId, data.FullName);

            // Bước 1: Cập nhật Database cục bộ của SignalR (nếu có)
            // var user = await _db.Users.FindAsync(data.UserId);
            // if(user != null) { user.FullName = data.FullName; await _db.SaveChangesAsync(); }

            // Bước 2: (Tùy chọn) Thông báo cho Client đang kết nối qua SignalR Hub
            // await _hubContext.Clients.User(data.UserId).SendAsync("ProfileUpdated", data);
        }
    }
}
