using CBVSignalR.Application.Base.Service;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Application.Models;
using CBVSignalR.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

namespace CBVSignalR.Application.Services
{
    public class GroupSubscriptionService : BaseService<GroupSubscription, Guid>, IGroupSubscriptionService
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
