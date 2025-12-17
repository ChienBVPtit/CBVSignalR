using CBVSignalR.Application.Base.Entity;
using CBVSignalR.Application.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace CBVSignalR.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<GroupSubscription> GroupSubscription { get; set; }
        public DbSet<UserGroupSubscription> UserGroupSubscription { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<InboxEvent> InboxEvent { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var stringProperties = entityType.ClrType
                                                 .GetProperties()
                                                 .Where(p => p.PropertyType == typeof(string));

                foreach (var property in stringProperties)
                {
                    // Kiểm tra xem property đã được cấu hình MaxLength chưa
                    var existingMaxLength = builder.Entity(entityType.ClrType)
                                                   .Property(property.Name)
                                                   .Metadata.GetMaxLength();

                    if (existingMaxLength == null)
                    {
                        // Nếu chưa khai báo MaxLength riêng, gán default
                        builder.Entity(entityType.ClrType)
                               .Property(property.Name)
                               .HasMaxLength(255); // giá trị mặc định
                    }
                }
            }

            builder.Entity<GroupSubscription>(entity =>
            {
                entity.Property(p => p.Id)
                      .IsRequired();

                entity.HasIndex(p => p.Id)
                      .IsUnique();

                entity.Property(p => p.Code)
                      .HasMaxLength(25)
                      .IsRequired();

                entity.HasIndex(p => p.Code)
                      .IsUnique();
            });

            builder.Entity<UserGroupSubscription>(entity =>
            {
                entity.HasOne(rp => rp.GroupSubscription)
                      .WithMany()
                      .HasForeignKey(rp => rp.GroupId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Notification>(entity =>
            {
                entity.Property(p => p.Id)
                      .IsRequired();

                entity.HasIndex(p => p.Id)
                      .IsUnique();

                entity.Property(p => p.Content)
                      .HasMaxLength(4000);
            });

            builder.Entity<InboxEvent>(entity =>
            {
                entity.Property(p => p.Id)
                      .IsRequired();

                entity.HasIndex(p => p.Id)
                      .IsUnique();
            });
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            //string? currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
            string? currentUser = "System";

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = currentUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUser;
                }
            }
        }
    }
}
