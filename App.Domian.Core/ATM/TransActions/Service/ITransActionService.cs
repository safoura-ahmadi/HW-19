using App.Domain.Core.ATM.TransActions.Dto;
using App.Domain.Core.ATM.TransActions.Entitiy;

namespace App.Domain.Core.ATM.TransActions.Service;

public interface ITransActionService
{
    void Add(MyTransAction transAction);
    List<TransActionDto> GetCardTransActions(int cardId);
    float MoneyTransferPerDay(int cardId);

}
