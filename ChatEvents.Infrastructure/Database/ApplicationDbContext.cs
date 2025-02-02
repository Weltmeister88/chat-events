using ChatEvents.ConfigurationOptions;
using ChatEvents.Models.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace ChatEvents.Infrastructure.Database;

public class ApplicationDbContext(IOptions<DatabaseOptions> databaseOptions) : DbContext
{
    private readonly DatabaseOptions _databaseOptions = databaseOptions.Value;
    
    public DbSet<ChatEventDbEntity> ChatEvents { get; set; }
    public DbSet<UserDbEntity> Users { get; set; }
    public DbSet<ChatRoomDbEntity> ChatRooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(_databaseOptions.DatabaseName);
        base.OnConfiguring(optionsBuilder);
    }

    public override int SaveChanges()
    {
        SetAuditingInfo();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetAuditingInfo();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditingInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditingInfo()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x is { Entity: AuditedDbEntity, State: EntityState.Added or EntityState.Modified });

        foreach (EntityEntry entity in entities)
        {
            var now = DateTimeOffset.UtcNow;

            var auditedEntity = (AuditedDbEntity)entity.Entity;
            if (entity.State == EntityState.Added && auditedEntity.CreatedUtc == default)
            {
                auditedEntity.CreatedUtc = now;
            }
            auditedEntity.ModifiedUtc = now;
        }
    }
}