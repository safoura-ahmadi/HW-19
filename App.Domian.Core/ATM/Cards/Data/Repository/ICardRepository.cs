using App.Domain.Core.ATM.Cards.Entitiy;

namespace App.Domain.Core.ATM.Cards.Data.Repository;

public interface ICardRepository
{
    public float GetBalance(int cardId);
    void ChangePassword(int cardId, string n);
    Card GetCardByNumber(string cardNumber);
    void BlockCard(int cardId);
    bool TransAction(Card sourceCard, Card destinationCard, float amount, float fee);
    bool ResetPasswordTry(int cardId);
    bool PasswordTryInc(int cardId);
    int GetCardTryPasswordCount(int cardId);
}
