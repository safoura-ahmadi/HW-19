using App.Domain.Core.ATM.TransActions.Entitiy;
using App.Domain.Core.ATM.Users.Entitiy;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Core.ATM.Cards.Entitiy;

public class Card
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(16)]
    public required string Number { get; set; }
    [MaxLength(25)]
    public string? HolderName { get; set; }
    public float Balance { get; set; }
    public bool IsActive { get; set; }
    [Required]
    [MaxLength(4)]
    public required string Password { get; set; }
    public int PasswordTryCount { get; set; }
    public int UserId { get; set; }
    public User? user { get; set; }
    // تراکنش‌هایی که این کارت مبدا آن‌ها است
    public List<MyTransAction> SourceTransactions { get; set; } = [];
    // تراکنش‌هایی که این کارت مقصد آن‌ها است
    public List<MyTransAction> DestinationTransactions { get; set; } = [];
}

