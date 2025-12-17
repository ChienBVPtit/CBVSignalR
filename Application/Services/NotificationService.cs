using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CBVSignalR.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _db;
        public NotificationService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Notification> CreateAsync(Notification entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _db.Notification.Add(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.Notification.FindAsync(id);
            if (existing == null) return false;

            _db.Notification.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _db.Notification
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _db.Notification
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Notification?> UpdateAsync(Guid id, Notification entity)
        {
            var existing = await _db.Notification.FindAsync(id);
            if (existing == null) return null;

            await _db.SaveChangesAsync();
            return existing;
        }
    }
}
