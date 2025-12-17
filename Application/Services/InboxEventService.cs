using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Context;
using Microsoft.EntityFrameworkCore;

namespace CBVSignalR.Application.Services
{
    public class InboxEventService : IInboxEventService
    {
        private readonly ApplicationDbContext _db;

        public InboxEventService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<InboxEvent> CreateAsync(InboxEvent entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _db.InboxEvent.Add(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.InboxEvent.FindAsync(id);
            if (existing == null) return false;

            _db.InboxEvent.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<InboxEvent>> GetAllAsync()
        {
            return await _db.InboxEvent
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InboxEvent?> GetByIdAsync(Guid id)
        {
            return await _db.InboxEvent
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<InboxEvent?> UpdateAsync(Guid id, InboxEvent entity)
        {
            var existing = await _db.InboxEvent.FindAsync(id);
            if (existing == null) return null;

            await _db.SaveChangesAsync();
            return existing;
        }
    }
}
