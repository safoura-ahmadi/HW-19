using App.Domain.Core.ATM.Cards.Entitiy;

namespace App.Domain.Core.ATM.Cards.Service;

public interface ICardService
{
    public float GetBalance(int cardId);
    public int GetCardTryPasswordCount(int cardId);
    bool ResetPasswordTry(int cardId);
    bool PasswordTryInc(int cardId);
    void ChangePassword(int cardId, string n);
    Card GetCardByNumber(string cardNumber);
    void BlockCard(int cardId);
    bool TransAction(Card sourceCard, Card destinationCard, float amount, float fee);
}
