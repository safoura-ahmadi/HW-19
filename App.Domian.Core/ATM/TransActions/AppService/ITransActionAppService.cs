using App.Domain.Core.ATM.Bases.Entitiy;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Domain.Core.ATM.TransActions.Dto;

namespace App.Domain.Core.ATM.TransActions.AppService;

public interface ITransActionAppService
{
    List<TransActionDto> GetCardTransActions(Card card);
    public float CalculateFeeTransfer(float money);
    Result TransferMoney(Card sourceCard, Card destinationCard, float money);
    bool VerificationCodeGnerator();
    public Result VerificationCodeMaching(string code, DateTime codeGnerationTime);
}
