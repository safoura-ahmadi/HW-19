

using App.Domain.Core.ATM.Bases.Entitiy;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Domain.Core.ATM.Cards.Service;
using App.Domain.Core.ATM.TransActions.AppService;
using App.Domain.Core.ATM.TransActions.Dto;
using App.Domain.Core.ATM.TransActions.Service;
using App.Domain.Core.ATM.TransActions.Entitiy;

namespace App.Domain.Services.AppService.ATM.TransActions;

public class TransActionAppService(ITransActionService transActionService, ICardService cardService) : ITransActionAppService
{
    private readonly string _path = "TransactionVerificationCode.txt";
    private readonly Random _random = new();
    private static DateTime ExpiredTime;
    private readonly ITransActionService _transActionService = transActionService;
    private readonly ICardService _cardService = cardService;

    public List<TransActionDto> GetCardTransActions(Card card)
        => _transActionService.GetCardTransActions(card.Id);

    public float CalculateFeeTransfer(float money)
    {
        if (money > 1000)
            return (float)1.5 * money / 100;
        return (float)0.5 * money / 100;
    }

    public Result TransferMoney(Card sourceCard, Card destinationCard, float money)
    {


        if (sourceCard == null)
            return new Result(false, "کارت مبدا یافت نشد");
        if (!sourceCard.IsActive)
            return new Result(false, "کارت مبدا مسدود است");
        if (destinationCard == null)
            return new Result(false, "کارت مقصد یافت نشد");
        if (!destinationCard.IsActive)
            return new Result(false, "کارت مقصد مسدود است");
        if (sourceCard.Id == destinationCard.Id)
            return new Result(false, "کارت کقصد نباید با کارت مبدا یکی باشد");
        if (money <= 0)
            return new Result(false, "مبلغ وارد شده معتبر نیست");
        if (sourceCard.Balance <= CalculateFeeTransfer(money) + money)
            return new Result(false, "کارت مبدا موجودی به حد کافی ندارد");
        if (_transActionService.MoneyTransferPerDay(sourceCard.Id) + money > 250)
            return new Result(false, "محدودیت تراکنش روزانه ی کارت مبدا بیش از سقف مجاز است");

        var isSuccess = _cardService.TransAction(sourceCard, destinationCard, money, CalculateFeeTransfer(money));
        var transaction = new MyTransAction()
        {
            SourceCardId = sourceCard.Id,
            DestinationCardId = destinationCard.Id,
            Amount = money,
            TransactionDate = DateOnly.FromDateTime(DateTime.Now),
            IsSuccessful = isSuccess
        };
        try
        {
            _transActionService.Add(transaction);
        }
        catch (Exception ex)
        {
            return new Result(false, ex.Message);
        }
        return new Result(true, "انتقال با موفقیت انجام شد");


    }

    public bool VerificationCodeGnerator()
    {

        int rndNum = _random.Next(10000, 100000);
        try
        {
            File.WriteAllText(_path, rndNum.ToString());
            ExpiredTime = DateTime.Now;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Result VerificationCodeMaching(string code, DateTime codeGnerationTime)
    {
        try
        {
            var fileCode = File.ReadAllText(_path);

            if (fileCode == code && codeGnerationTime.AddMinutes(2) >= DateTime.Now)
                return new Result(true);

            if (fileCode == code && codeGnerationTime.AddMinutes(2) < DateTime.Now)
                return new Result(false, "کد منقضی شده است، برای تولید کد جدید دوباره امتحان کنید");

            return new Result(false, "کد معتبر نیست");
        }
        catch (Exception ex)
        {
            return new Result(false, $"There is Some Errors in File: {ex.Message}");
        }
    }
}
