namespace App.Domain.Core.ATM.TransActions.Dto;

public class TransActionDto
{
    public required string SourceCardNumber { get; set; }
    public required string DestinationCardNUmber { get; set; }
    public float Amount { get; set; }
    public DateOnly TransactionDate { get; set; }
    public bool IsSuccessful { get; set; }
}
