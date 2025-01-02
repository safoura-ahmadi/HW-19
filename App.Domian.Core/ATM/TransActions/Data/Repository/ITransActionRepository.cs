using App.Domain.Core.ATM.TransActions.Dto;
using App.Domain.Core.ATM.TransActions.Entitiy;

namespace App.Domain.Core.ATM.TransActions.Data.Repository;
public interface ITransActionRepository
{
    void Add(MyTransAction transAction);
    List<TransActionDto> GetCardTransActions(int cardId);
    float MoneyTransferPerDay(int cardId);

}
