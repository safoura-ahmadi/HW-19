using App.Domain.Core.ATM.Cards.AppService;
using App.Domain.Core.ATM.Cards.Data.Repository;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Domain.Core.ATM.Cards.Service;
using App.Domain.Core.ATM.TransActions.AppService;
using App.Domain.Core.ATM.TransActions.Data.Repository;
using App.Domain.Core.ATM.TransActions.Dto;
using App.Domain.Core.ATM.TransActions.Service;
using App.Domain.Core.ATM.Users.AppService;
using App.Domain.Core.ATM.Users.Data.Repository;
using App.Domain.Core.ATM.Users.Service;
using App.Domain.Services.AppService.ATM.Cards;
using App.Domain.Services.AppService.ATM.TransActions;
using App.Domain.Services.AppService.ATM.Users;
using App.Domain.Services.Service.ATM.Cards;
using App.Domain.Services.Service.ATM.TransActions;
using App.Domain.Services.Service.ATM.Users;
using App.Infrastructure.DbAccess.Repository.Ef.ATM.Cards;
using App.Infrastructure.DbAccess.Repository.Ef.ATM.TransActions;
using App.Infrastructure.DbAccess.Repository.Ef.ATM.Users;
using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    // User Dependencies
    .AddScoped<IUserRepository, UserEfRepository>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IUserAppService, UserAppService>()
    // Card Dependencies
    .AddScoped<ICardRepository, CardEfRepository>()
    .AddScoped<ICardService, CardService>()
    .AddScoped<ICardAppService, CardAppService>()
    // Transaction Dependencies
    .AddScoped<ITransActionRepository, TransActionEfRepository>()
    .AddScoped<ITransActionService, TransActionService>()
    .AddScoped<ITransActionAppService, TransActionAppService>()
    .BuildServiceProvider();

// Resolve Services
var userAppService = serviceProvider.GetRequiredService<IUserAppService>();
var cardAppService = serviceProvider.GetRequiredService<ICardAppService>();
var transActionAppService = serviceProvider.GetRequiredService<ITransActionAppService>();


MainMenu();
void MainMenu()
{
    Console.Clear();
    Console.WriteLine("1-Fund Transfer");
    Console.WriteLine("2-Transfer Report");
    Console.WriteLine("3-Card Balance");
    Console.WriteLine("4-Change Card Password");
    Console.Write("Enter Number: ");
    if (!Int32.TryParse(Console.ReadLine(), out int number))
    {
        Console.WriteLine("Invalid Number.");
        Console.ReadKey();
    }
    switch (number)
    {
        case 1:
            Console.Clear();
            FundTransfer();
            break;
        case 2:
            Console.Clear();
            CardTransActions();
            break;
        case 3:
            Console.Clear();
            CardBalance();
            break;
        case 4:
            Console.Clear();
            ChangePassword();
            break;
        default:
            Console.WriteLine("You Must Entered Valid Number");
            Console.ReadKey();
            Console.Clear();
            break;
    }


    MainMenu();
}
void FundTransfer()
{
    #region GetardsInformation
    var sourceCard = GetCard();

    Console.Write("Enter Destination Card Number: ");
    var destinationNumber = Console.ReadLine();
    var result = cardAppService.Get(destinationNumber ?? "", out Card? destinationCard);

    if (!result.Success)
    {
        Console.WriteLine(result.Message);
        Console.ReadKey();
        MainMenu();
    }
    if (destinationCard != null)
    {
        Console.Write("Enter Amount Of Money That You Wanna Transfer: ");
        if (!float.TryParse(Console.ReadLine(), out float money) || money <= 0)
        {
            Console.WriteLine("The amount you entered is invalid.");
            Console.ReadKey();
            MainMenu();
        }
        #endregion
        Console.Clear();
        Console.WriteLine("****************************************");
        Console.WriteLine($"Source Code Number: {sourceCard.Number}");
        Console.WriteLine($"Destination Code Number: {destinationCard.Number}");
        Console.WriteLine($"Destination Card Holder's Name: {userAppService.GetName(destinationCard.UserId)}");
        Console.WriteLine($"Final Amount: {transActionAppService.CalculateFeeTransfer(money) + money}");
        Console.WriteLine("****************************************");
        Console.Write("Confirm?y/n ");
        var answer = Console.ReadLine();
        if (answer != null && (answer == "n" || answer == "N" || answer.Equals("NO", StringComparison.CurrentCultureIgnoreCase)))
        {
            Console.WriteLine("Money Transfer Cancelled.");
            Console.ReadKey();
            MainMenu();
        }
        else if (answer != null && (answer == "y" || answer == "Y" || answer.ToString().Equals("YES", StringComparison.CurrentCultureIgnoreCase)))
        {
            var generateCodeResult = transActionAppService.VerificationCodeGnerator();
            DateTime codeGenerationTime = DateTime.Now;
            DateTime codeExpiredTime = codeGenerationTime.AddMinutes(2);
            if (generateCodeResult)
            {
                do
                {
                    Console.Clear();
                    Console.Write("Enter VerificationCode to Finalize TransAction: ");
                    var code = Console.ReadLine();
                    var machingCodeResult = transActionAppService.VerificationCodeMaching(code ?? "", codeGenerationTime);
                    if (machingCodeResult.Success)
                    {
                        result = transActionAppService.TransferMoney(sourceCard, destinationCard, money);
                        Console.WriteLine(result.Message);
                        Console.ReadKey();
                        MainMenu();
                    }
                    else
                        Console.WriteLine(machingCodeResult.Message);
                    Console.ReadKey();

                } while (codeExpiredTime > DateTime.Now);
                Console.WriteLine("Oops The Code Has Expired. For Generating New One Try Again Later. ");
                Console.ReadKey();
                MainMenu();

            }
            else
            {
                Console.WriteLine("Somthing Wrong Happen in file While Generating Code.");
                Console.ReadKey();
                MainMenu();
            }
        }

        else
        {
            Console.WriteLine("Answer Not Valid.");
            Console.ReadKey();
            MainMenu();
        }

    }
}
void CardTransActions()
{
    var card = GetCard();
    if (!card.IsActive)
    {
        Console.WriteLine("This Card Is Block.");
        Console.ReadKey();
        return;
    }
    var transactions = transActionAppService.GetCardTransActions(card);

    ConsoleTable
        .From<TransActionDto>(transactions)
        .Configure(o => o.NumberAlignment = Alignment.Right)
        .Write(Format.Minimal);

    Console.ReadKey();
}
Card GetCard()
{
    Console.Write("enter your card number: ");
    var sourceNumber = Console.ReadLine();
    var result = cardAppService.Get(sourceNumber ?? "", out Card? card);
    if (!result.Success)
    {
        Console.WriteLine(result.Message);
        Console.ReadKey();
        MainMenu();

    }

    do
    {
        Console.Write("enter Your  Card Password: ");
        var password = Console.ReadLine();
        if (card != null)
        {
            if (cardAppService.Block(card))
            {
                Console.WriteLine("Oops Your Card Blocked.");
                Console.ReadKey();
                MainMenu();
            }
            else if (!cardAppService.PasswordIsValid(card, password ?? ""))
            {
                Console.WriteLine("Password Wrong.Take Care that After 3 Wrong Password Your Card Will Be Blocked.");

            }
            else
                return card;
            Console.ReadKey();
            Console.Clear();
        }
    } while (true);
}
void CardBalance()
{
    var card = GetCard();
    var result = cardAppService.GetBalance(card, out float balance);
    if (!result.Success)
        Console.WriteLine(result.Message);
    else
        Console.WriteLine($"Blance: {balance}");
    Console.ReadKey();
    MainMenu();
}
void ChangePassword()
{
    var card = GetCard();
    Console.Write("Enter New Password: ");
    var newPass = Console.ReadLine();
    var result = cardAppService.ChangePassword(card, newPass ?? card.Password);
    Console.WriteLine(result.Message);
    Console.ReadKey();
    MainMenu();
}
