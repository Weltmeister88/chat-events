namespace ChatEvents.Models.DbEntities;

public abstract class AuditedDbEntity : DbEntity
{
    public DateTimeOffset CreatedUtc { get; set; }
    public DateTimeOffset ModifiedUtc { get; set; }
}