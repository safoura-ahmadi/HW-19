

using App.Domain.Core.ATM.TransActions.Data.Repository;
using App.Domain.Core.ATM.TransActions.Dto;
using App.Domain.Core.ATM.TransActions.Entitiy;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.DbAccess.Repository.Ef.ATM.TransActions;

public class TransActionEfRepository : ITransActionRepository

{
    private readonly AppDbContext _context;
    public TransActionEfRepository()
    {
        _context = new AppDbContext();
    }

    public void Add(MyTransAction transAction)
    {
        _context.TransActions.Add(transAction);
        _context.SaveChanges();
    }

    public float MoneyTransferPerDay(int cardId)
    {
        return _context.TransActions
            .Where(t => t.SourceCardId == cardId && t.TransactionDate == DateOnly.FromDateTime(DateTime.Now)
                   && t.IsSuccessful == true)
            .Sum(t => t.Amount);
    }

    List<TransActionDto> ITransActionRepository.GetCardTransActions(int cardnumber)
    {
        var transActions = _context.TransActions.AsNoTracking()
          .Where(t => t.SourceCardId == cardnumber || t.DestinationCardId == cardnumber)
          .Include(t => t.SourceCard)
          .Include(t => t.DestinationCard)
          .Select(t => new TransActionDto()
          {
              SourceCardNumber = t.SourceCard.Number,
              DestinationCardNUmber = t.DestinationCard.Number,
              Amount = t.Amount,
              TransactionDate = t.TransactionDate,
              IsSuccessful = t.IsSuccessful

          }
          )
          .ToList();
        return transActions ?? [];
    }
}
