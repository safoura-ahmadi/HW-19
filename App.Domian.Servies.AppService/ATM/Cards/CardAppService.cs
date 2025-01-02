using App.Domain.Core.ATM.Bases.Entitiy;
using App.Domain.Core.ATM.Cards.AppService;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Domain.Core.ATM.Cards.Service;

namespace App.Domain.Services.AppService.ATM.Cards;

public class CardAppService(ICardService cardService) : ICardAppService
{
    private readonly ICardService _cardService = cardService;

    public bool Block(Card card)
    {
        if (card is null)
            return false;
        if (_cardService.GetCardTryPasswordCount(card.Id) <= 3)
            return false;
        try
        {
            _cardService.BlockCard(card.Id);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Result ChangePassword(Card card, string newPassword)
    {
        if (card is null)
            return new Result(false, "ابتدا مشخصات کارت را وارد کنید");
        if (!card.IsActive)
            return new Result(false, "این کارت مسدود است");
        if (newPassword.Length != 4 || !newPassword.All(char.IsDigit))
            return new Result(false, "رمز باید دقیقا چهار رقم باشد");
        try
        {
            _cardService.ChangePassword(card.Id, newPassword);
            return new Result(true, "رمز با موفقیت تغییر کرد");
        }
        catch (Exception ex)
        {
            return new Result(false, $"Somthing Happen in DataBase:{ex.Message}");
        }
    }

    public Result Get(string cardNumber, out Card? card)
    {

        if (cardNumber.Length != 16)
        {
            card = null;
            return new Result(false, "شماره کارت وارد شده نامعتبر است.");
        }
        try
        {
            card = _cardService.GetCardByNumber(cardNumber);
            if (card.IsActive)
                return new Result(true);
            else
                return new Result(false, "کارت مسدود است");
        }
        catch
        {
            card = null;
            return new Result(false, "شماره کارت وارد شده موجود نیست.");
        }
    }

    public Result GetBalance(Card card, out float balance)
    {
        if (card is null)
        {
            balance = 0;
            return new Result(false, "ابتدا مشخصات کارت را وارد کنید");
        }
        if (!card.IsActive)
        {
            balance = 0;
            return new Result(false, "این کارت مسدود است");
        }
        else
        {
            balance = _cardService.GetBalance(card.Id);
            return new Result(true);
        }
    }

    public bool PasswordIsValid(Card card, string password)
    {
        _cardService.PasswordTryInc(card.Id);

        if (card.Password == password)
        {
            _cardService.ResetPasswordTry(card.Id);
            return true;
        }

        return false;
    }

}
