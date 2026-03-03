using CBVSignalR.Application.Entities;
using CBVSignalR.Context;
using CBVSignalR.Events.ModelEvent;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CBVSignalR.Events.Consumers
{
    public class UsersSyncConsumer : IConsumer<IUsersSyncEvent>
    {
        private readonly ApplicationDbContext _db; 
        private readonly ILogger<UsersSyncConsumer> _logger;

        public UsersSyncConsumer(ApplicationDbContext db, ILogger<UsersSyncConsumer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IUsersSyncEvent> context)
        {
            var incomingUsers = context.Message.Users;
            if (incomingUsers == null || !incomingUsers.Any()) return;

            // 1. Lấy danh sách ID từ event
            var incomingIds = incomingUsers.Select(u => u.UserId).ToList();

            // 2. Lấy các User đã tồn tại trong DB local
            var existingUsers = await _db.User
                .Where(u => incomingIds.Contains(u.UserId.ToString()))
                .ToDictionaryAsync(u => u.Id); // Dùng Dictionary để tra cứu nhanh hơn O(1)

            var usersToUpdate = new List<User>();
            var usersToInsert = new List<User>();
            _logger.LogInformation($"Bắt đầu thực hiện đồng bộ danh sách User");
            // 3. Phân loại và Mapping
            foreach (var incoming in incomingUsers)
            {
                if (existingUsers.TryGetValue(Guid.Parse(incoming.UserId), out var existingUser))
                {
                    // Cập nhật thông tin (Update)
                    existingUser.FullName = incoming.FullName;
                    existingUser.Email = incoming.Email;
                    existingUser.UserName = incoming.UserName;
                    existingUser.PhoneNumber = incoming.PhoneNumber;
                    usersToUpdate.Add(existingUser);
                    _logger.LogInformation($"Thực hiện đồng bộ danh sách User - Cập nhật {existingUser}");
                }
                else
                {
                    // Thêm mới (Insert)
                    usersToInsert.Add(new User
                    {
                        UserId = Guid.Parse(incoming.UserId),
                        FullName = incoming.FullName,
                        Email = incoming.Email,
                        UserName = incoming.UserName,
                        PhoneNumber = incoming.PhoneNumber
                    });
                    _logger.LogInformation($"Thực hiện đồng bộ danh sách User - Thêm mới {incoming}");
                }
            }

            // 4. Lưu vào Database
            if (usersToInsert.Any())
            {
                await _db.User.AddRangeAsync(usersToInsert);
            }

            // EF Core sẽ tự theo dõi trạng thái của existingUsers trong list usersToUpdate
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Đồng bộ hoàn tất: Thêm mới {usersToInsert.Count}, Cập nhật {usersToUpdate.Count}");
        }

    }
}
