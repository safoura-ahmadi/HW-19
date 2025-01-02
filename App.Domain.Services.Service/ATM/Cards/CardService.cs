using App.Domain.Core.ATM.Cards.Data.Repository;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Domain.Core.ATM.Cards.Service;

namespace App.Domain.Services.Service.ATM.Cards;

public class CardService(ICardRepository cardReposirory) : ICardService
{
    public void BlockCard(int cardId)
    {
       cardReposirory.BlockCard(cardId);
    }
    public int GetCardTryPasswordCount(int cardId)
    {

        return cardReposirory.GetCardTryPasswordCount(cardId);

    }

    public void ChangePassword(int cardId, string newPassword)
    {
        cardReposirory.ChangePassword(cardId, newPassword);
    }

    public Card GetCardByNumber(string cardNumber)
    {
       return cardReposirory.GetCardByNumber(cardNumber);
    }

    public bool PasswordTryInc(int cardId)
    {
       return cardReposirory.PasswordTryInc(cardId);
    }

    public bool ResetPasswordTry(int cardId)
    {
        return cardReposirory.ResetPasswordTry(cardId);
    }

    public bool TransAction(Card sourceCard, Card destinationCard, float amount, float fee)
    {
       return cardReposirory.TransAction(sourceCard, destinationCard, amount, fee);
    }

    public float GetBalance(int cardId)
    {
        return cardReposirory.GetBalance(cardId);
    }
}
