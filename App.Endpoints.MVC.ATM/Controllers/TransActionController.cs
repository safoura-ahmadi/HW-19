using App.Domain.Core.ATM.Bases.Entitiy;
using App.Domain.Core.ATM.Cards.AppService;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Domain.Core.ATM.TransActions.AppService;
using App.Domain.Core.ATM.Users.AppService;
using App.Domain.Services.AppService.ATM.Cards;
using App.Endpoints.MVC.ATM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Endpoints.MVC.ATM.Controllers
{
    public class TransActionController(ITransActionAppService transActionAppService, ICardAppService cardAppService, IUserAppService userAppService) : Controller
    {
        private readonly ITransActionAppService _transActionAppService = transActionAppService;
        private readonly ICardAppService _cardAppService = cardAppService;
        private readonly IUserAppService _userAppService = userAppService;

        public IActionResult GetCardTransActions()
        {
            var card = HttpContext.Session.GetObject<Card>("card");
            if (card == null)
            {
                return RedirectToAction("Index", "Card");
            }

            ViewData["Title"] = "تاریخچه ی انتقال";
            ViewBag.CssFile = "getTransAction.css";
            var transActionList = _transActionAppService.GetCardTransActions(card);
            return View(transActionList);

        }
        public IActionResult Transfer()
        {
            var card = HttpContext.Session.GetObject<Card>("card");
            if (card == null)
            {
                return RedirectToAction("Index", "Card");
            }
            ViewData["Title"] = "کارت به کارت";
            ViewBag.CssFile = "transfer.css";
            return View(card);
        }
        [HttpPost]
        public IActionResult TransferMoney(string password, string destinationCardNumber, float money)
        {
            var sourceCard = HttpContext.Session.GetObject<Card>("card");
            if (sourceCard == null)
            {
                return RedirectToAction("Index", "Card");
            }
            if (sourceCard.Password != password)
            {
                TempData["ErrorMessage"] = "رمز اشتباه است";
                return RedirectToAction("Transfer", "TransAction");
            }

            var result = _cardAppService.Get(destinationCardNumber, out Card? destinationCard);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Transfer", "TransAction");
            }
            if (money <= 0)
            {
                TempData["ErrorMessage"] = "مبغ وارد شده نامعبتر است";
                return RedirectToAction("Transfer", "TransAction");
            }
            if(destinationCard != null && destinationCard.Number == sourceCard.Number)
            {
                TempData["ErrorMessage"] = "کارت مقصد نباید با کارت مبدا یکسان باشد";
                return RedirectToAction("Transfer", "TransAction");
            }
            HttpContext.Session.SetObject("destinationCard", destinationCard);
            return RedirectToAction("TransferCompletement", "TransAction", new { money });



        }
        public IActionResult TransferCompletement(float money)
        {
            ViewData["Title"] = "کارت به کارت";
            ViewBag.CssFile = "transfer.css";

            var sourceCard = HttpContext.Session.GetObject<Card>("card");
            if (sourceCard == null)
            {
                return RedirectToAction("Index", "Card");
            }
            var destinationCard = HttpContext.Session.GetObject<Card>("destinationCard");
            if (destinationCard != null)
            {

                var codeResult = _transActionAppService.VerificationCodeGnerator();
                var codeGenerateTime = DateTime.Now;
                if (!codeResult)
                {
                    TempData["ErrorMessage"] = "Some Trouble Was Detected in File";
                    return RedirectToAction("Transfer", "TransAction");
                }
                var transferMOneyMv = new TransferMoneyMv()
                {
                    SourceCardNumber = sourceCard.Number,
                    DestinationCardNumber = destinationCard.Number,
                    DestinationHolderName = _userAppService.GetName(destinationCard.UserId),
                    Money = money,
                    CodeGenerateTime = codeGenerateTime,
                };

                return View(transferMOneyMv);

            }
            return RedirectToAction("Transfer", "TransAction");


        }
        [HttpPost]

        public IActionResult CardTransferCompletement(TransferMoneyMv transferMoneyMv)
        {
            var sourceCard = HttpContext.Session.GetObject<Card>("card");
            var dCard = HttpContext.Session.GetObject<Card>("destinationCard");
            HttpContext.Session.Remove("destanitionCard");
            var codeResult = _transActionAppService.VerificationCodeMaching(transferMoneyMv.Code ?? "", transferMoneyMv.CodeGenerateTime);
            if (!codeResult.Success)
            {
                TempData["ErrorMessage"] = codeResult.Message;
                return RedirectToAction("TransferCompletement", "TransAction");
            }
            if (sourceCard is not null && dCard is not null)
            {
                var tranferResult = _transActionAppService.TransferMoney(sourceCard, dCard, transferMoneyMv.Money);
                if (!tranferResult.Success)
                {
                    TempData["ErrorMessage"] = tranferResult.Message;
                    return RedirectToAction("TransferCompletement", "TransAction",new { transferMoneyMv.Money});
                }
                TempData["SuccessMessage"] =  tranferResult.Message;
                return RedirectToAction("Menu", "Card");
            }
            TempData["ErrorMessage"] = "کارت مبدا یا مقصد یافت نشد";
            return RedirectToAction("TransferCompletement", "TransAction");
        }
    }
}

