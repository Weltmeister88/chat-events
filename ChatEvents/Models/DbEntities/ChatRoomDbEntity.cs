using System.ComponentModel.DataAnnotations.Schema;

namespace ChatEvents.Models.DbEntities;

[Serializable]
[Table("ChatRooms")]
public class ChatRoomDbEntity : AuditedDbEntity
{
    public string Name { get; set; } = null!;
}