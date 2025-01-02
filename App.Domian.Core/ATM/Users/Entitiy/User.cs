using App.Domain.Core.ATM.Cards.Entitiy;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Core.ATM.Users.Entitiy;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(25)]
    public required string Name { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    public DateOnly Birthday { get; set; }
    public List<Card> Cards { get; set; } = [];
}
