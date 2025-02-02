using System.ComponentModel.DataAnnotations.Schema;

namespace ChatEvents.Models.DbEntities;

[Serializable]
[Table("Users")]
public class UserDbEntity : AuditedDbEntity
{
    public string Name { get; set; } = null!;
}