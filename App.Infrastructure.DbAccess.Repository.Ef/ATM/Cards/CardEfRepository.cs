using App.Domain.Core.ATM.Cards.Data.Repository;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.DbAccess.Repository.Ef.ATM.Cards;

public class CardEfRepository : ICardRepository
{
    private readonly AppDbContext _context;
    public CardEfRepository()
    {
        _context = new AppDbContext();
    }

    public void BlockCard(int cardId)
    {
        var cardd = _context.Cards.First(c => c.Id == cardId);
        cardd.IsActive = false;
        _context.SaveChanges();
    }

    public void ChangePassword(int cardId, string newPassword)
    {
        var card = _context.Cards.First(c => c.Id == cardId);
        card.Password = newPassword;
        _context.SaveChanges();
    }

    public Card GetCardByNumber(string cardNumber)
    {
        return _context.Cards.First(c => c.Number == cardNumber);
    }

    public int GetCardTryPasswordCount(int cardId)
    {

        return _context.Cards.AsNoTracking().First(c => c.Id == cardId).PasswordTryCount;

    }

    public bool PasswordTryInc(int cardId)
    {
        try
        {
            var card = _context.Cards.First(c => c.Id == cardId);
            card.PasswordTryCount++;
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ResetPasswordTry(int cardId)
    {
        try
        {
            var card = _context.Cards.First(c => c.Id == cardId);
            card.PasswordTryCount = 0;
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool TransAction(Card sourceCard, Card destinationCard, float amount, float fee)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var scard = _context.Cards.First(c => c.Id == sourceCard.Id);
            var dcard = _context.Cards.First(c => c.Id == destinationCard.Id);
            scard.Balance -= amount + fee;
            dcard.Balance += amount;

            _context.SaveChanges();
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }
    public float GetBalance(int cardId)
    {
        return _context.Cards.AsNoTracking().First(c => c.Id == cardId).Balance;

    }
}
