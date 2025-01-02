using App.Domain.Core.ATM.Bases.Entitiy;
using App.Domain.Core.ATM.Cards.Entitiy;

namespace App.Domain.Core.ATM.Cards.AppService;

public interface ICardAppService
{
    public bool PasswordIsValid(Card card, string password);
    public Result GetBalance(Card card, out float balance);
    public Result Get(string cardNumber, out Card? card);
    public Result ChangePassword(Card card, string newPassword);
    public bool Block(Card card);
}
