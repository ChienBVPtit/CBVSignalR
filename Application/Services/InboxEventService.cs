using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Application.Models.App;
using CBVSignalR.Context;
using Microsoft.EntityFrameworkCore;

namespace CBVSignalR.Application.Services
{
    public class InboxEventService : BaseService<InboxEvent, Guid, InboxEventFilterRequest>, IInboxEventService
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

        //protected override IQueryable<GroupSubscription> ApplySort(
        //    IQueryable<GroupSubscription> query,
        //    PagingFilterRequest request)
        //{
        //    if (string.IsNullOrWhiteSpace(request.SortBy))
        //        return query.OrderByDescending(x => x.CreatedAt); // default

        //    return (request.SortBy.ToLower(), request.SortDir?.ToLower()) switch
        //    {
        //        ("name", "asc") => query.OrderBy(x => x.Name),
        //        ("name", "desc") => query.OrderByDescending(x => x.Name),

        //        ("code", "asc") => query.OrderBy(x => x.Code),
        //        ("code", "desc") => query.OrderByDescending(x => x.Code),

        //        ("createdat", "asc") => query.OrderBy(x => x.CreatedAt),
        //        ("createdat", "desc") => query.OrderByDescending(x => x.CreatedAt),

        //        _ => query.OrderByDescending(x => x.CreatedAt)
        //    };
        //}
    }
}
