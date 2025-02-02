using System.ComponentModel.DataAnnotations.Schema;

namespace ChatEvents.Models.DbEntities;

public abstract class DbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
}