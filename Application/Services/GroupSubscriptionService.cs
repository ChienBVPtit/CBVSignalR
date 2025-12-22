using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Application.Models;
using CBVSignalR.Application.Models.App;
using CBVSignalR.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

namespace CBVSignalR.Application.Services
{
    public class GroupSubscriptionService : BaseService<GroupSubscription, Guid, GroupSubscriptionFilterRequest>, IGroupSubscriptionService
    {
        protected ApplicationDbContext _db
        => (ApplicationDbContext)_context;
        public GroupSubscriptionService(ApplicationDbContext context)
        : base(context)
        {
        }
        public override async Task<GroupSubscription> CreateAsync(GroupSubscription entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // Trim strings để tránh duplicate vì space
            entity.Name = entity.Name.Trim();
            entity.Code = entity.Code.Trim();

            _db.GroupSubscription.Add(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.GroupSubscription.FindAsync(id);
            if (existing == null) return false;

            _db.GroupSubscription.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public override async Task<IEnumerable<GroupSubscription>> GetAllAsync()
        {
            return await _db.GroupSubscription
               .AsNoTracking()
               .ToListAsync();
        }

        public override async Task<GroupSubscription?> GetByIdAsync(Guid id)
        {
            return await _db.GroupSubscription
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<GroupSubscription?> UpdateAsync(Guid id, GroupSubscription entity)
        {
            var existing = await _db.GroupSubscription.FindAsync(id);
            if (existing == null) return null;

            existing.Name = entity.Name?.Trim() ?? existing.Name;
            existing.Code = entity.Code?.Trim() ?? existing.Code;
            existing.Description = entity.Description;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<GroupSubscription> GetGroupSubscriptionByName(string name)
        {
            name = name.Trim();
            var existing = await _db.GroupSubscription.FirstOrDefaultAsync(p => p.Name == name);
            if (existing == null) return null;
            return existing;
        }

        protected override IQueryable<GroupSubscription> ApplyFilter(
        IQueryable<GroupSubscription> query,
        GroupSubscriptionFilterRequest request)
        {
            //if (request.Keyword)
            //    query = query.Where(x => x.Type == request.Type);

            return query;
        }

        protected override IQueryable<GroupSubscription> ApplySearch(
            IQueryable<GroupSubscription> query,
            string? keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x =>
                    x.Code.Contains(keyword) ||
                    x.Name.Contains(keyword));

            return query;
        }

        protected override IQueryable<GroupSubscription> ApplySort(
            IQueryable<GroupSubscription> query,
            PagingFilterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SortBy))
                return query.OrderByDescending(x => x.CreatedAt); // default

            return (request.SortBy.ToLower(), request.SortDir?.ToLower()) switch
            {
                ("name", "asc") => query.OrderBy(x => x.Name),
                ("name", "desc") => query.OrderByDescending(x => x.Name),

                ("code", "asc") => query.OrderBy(x => x.Code),
                ("code", "desc") => query.OrderByDescending(x => x.Code),

                ("createdat", "asc") => query.OrderBy(x => x.CreatedAt),
                ("createdat", "desc") => query.OrderByDescending(x => x.CreatedAt),

                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }
    }
}
