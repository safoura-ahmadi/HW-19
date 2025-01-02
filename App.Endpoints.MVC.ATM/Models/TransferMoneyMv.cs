namespace App.Endpoints.MVC.ATM.Models;

public class TransferMoneyMv
{
    public required string SourceCardNumber { get; set; }
    public required string DestinationCardNumber { get; set; }
    public required string DestinationHolderName { get; set; }
    public required float Money { get; set; }
    public required  DateTime CodeGenerateTime { get; set; }
    public string? Code { get; set; }
}
