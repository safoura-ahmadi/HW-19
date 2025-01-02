using App.Domain.Core.ATM.TransActions.Data.Repository;
using App.Domain.Core.ATM.TransActions.Dto;
using App.Domain.Core.ATM.TransActions.Entitiy;
using App.Domain.Core.ATM.TransActions.Service;

namespace App.Domain.Services.Service.ATM.TransActions;

public class TransActionService(ITransActionRepository transActionRepository) : ITransActionService

{
    private readonly ITransActionRepository _transActionRepository = transActionRepository;

    public void Add(MyTransAction transAction)
    {
        _transActionRepository.Add(transAction);
    }

    public List<TransActionDto> GetCardTransActions(int cardId)
    {
        return _transActionRepository.GetCardTransActions(cardId);
    }

    public float MoneyTransferPerDay(int cardId)
    {
        return _transActionRepository.MoneyTransferPerDay(cardId);
    }


}
