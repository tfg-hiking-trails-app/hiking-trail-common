using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Length(36, 36)]
    [Column("code")]
    public Guid Code { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }
}