using App.Domain.Core.ATM.Cards.Entitiy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.ATM.TransActions.Entitiy;

public class MyTransAction
{
    [Key]
    public int Id { get; set; }
    public int SourceCardId { get; set; }
    [MaxLength(16)]
    public string? SourceCardNumber { get; set; }
    public int DestinationCardId { get; set; }
    [MaxLength(16)]
    public string? DestinationCardNUmber { get; set; }
    public float Amount { get; set; }
    public DateOnly TransactionDate { get; set; }
    public bool IsSuccessful { get; set; }
    public Card? SourceCard { get; set; }
    public Card? DestinationCard { get; set; }

}

