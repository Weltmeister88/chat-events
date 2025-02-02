using System.ComponentModel.DataAnnotations.Schema;

namespace ChatEvents.Models.DbEntities;

[Serializable]
[Table("ChatEvents")]
public class ChatEventDbEntity : AuditedDbEntity
{
    public long UserId { get; set; }
    public EventType EventType { get; set; }
    public string? Comment { get; set; }
    public long ChatRoomId { get; set; }
    public long? HighFivedUserId { get; set; }
    
    public UserDbEntity? HighFivedUser { get; set; }
    public UserDbEntity User { get; set; } = null!;
    public ChatRoomDbEntity ChatRoom { get; set; } = null!;
}