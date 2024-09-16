using System.ComponentModel.DataAnnotations;

namespace Core.Models.Entities;

public class EntityBase
{
    [Key]
    [Required]
    public int Id { get; set; }
}