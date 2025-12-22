using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Context;
using Microsoft.EntityFrameworkCore;

namespace CBVSignalR.Application.Services
{
    public class InboxEventService : BaseService<InboxEvent, Guid>, IInboxEventService
    {
        protected ApplicationDbContext _db
        => (ApplicationDbContext)_context;
        public InboxEventService(ApplicationDbContext context)
        : base(context)
        {
        }
        public override async Task<InboxEvent> CreateAsync(InboxEvent entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _db.InboxEvent.Add(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.InboxEvent.FindAsync(id);
            if (existing == null) return false;

            _db.InboxEvent.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public override async Task<IEnumerable<InboxEvent>> GetAllAsync()
        {
            return await _db.InboxEvent
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<InboxEvent?> GetByIdAsync(Guid id)
        {
            return await _db.InboxEvent
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<InboxEvent?> UpdateAsync(Guid id, InboxEvent entity)
        {
            var existing = await _db.InboxEvent.FindAsync(id);
            if (existing == null) return null;

            await _db.SaveChangesAsync();
            return existing;
        }

        //protected override IQueryable<GroupSubscription> ApplyFilter(
        //IQueryable<GroupSubscription> query,
        //PagingFilterRequest request)
        //{
        //    //if (request.Type.HasValue)
        //    //    query = query.Where(x => x.Type == request.Type);

        //    return query;
        //}

        //protected override IQueryable<GroupSubscription> ApplySearch(
        //    IQueryable<GroupSubscription> query,
        //    string? keyword)
        //{
        //    if (!string.IsNullOrWhiteSpace(keyword))
        //        query = query.Where(x =>
        //            x.Code.Contains(keyword) ||
        //            x.Name.Contains(keyword));

        //    return query;
        //}
    }
}
