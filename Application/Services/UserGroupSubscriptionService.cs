using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Context;
using Microsoft.EntityFrameworkCore;

namespace CBVSignalR.Application.Services
{
    public class UserGroupSubscriptionService : IUserGroupSubscriptionService
    {
        private readonly ApplicationDbContext _db;

        public UserGroupSubscriptionService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<UserGroupSubscription> CreateAsync(UserGroupSubscription entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _db.UserGroupSubscription.Add(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.UserGroupSubscription.FindAsync(id);
            if (existing == null) return false;

            _db.UserGroupSubscription.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserGroupSubscription>> GetAllAsync()
        {
            return await _db.UserGroupSubscription
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserGroupSubscription?> GetByIdAsync(Guid id)
        {
            return await _db.UserGroupSubscription
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<UserGroupSubscription?> UpdateAsync(Guid id, UserGroupSubscription entity)
        {
            var existing = await _db.UserGroupSubscription.FindAsync(id);
            if (existing == null) return null;

            await _db.SaveChangesAsync();
            return existing;
        }
    }
}
