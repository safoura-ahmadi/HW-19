using App.Domain.Core.ATM.Cards.AppService;
using App.Domain.Core.ATM.Cards.Entitiy;
using App.Endpoints.MVC.ATM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace App.Endpoints.MVC.ATM.Controllers
{
    public class CardController(ICardAppService cardAppService) : Controller
    {
        private readonly ICardAppService _cardAppService = cardAppService;

        public IActionResult Index()
        {
         
            ViewData["Title"] = "خوش امدید";
            return View();
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "صفحه ورود";
            ViewBag.CssFile = "login.css";
            return View();
        }
        [HttpPost]
        public IActionResult LoginCard(string cardNumber, string password)
        {
            HttpContext.Session.Remove("card");
           var result = _cardAppService.Get(cardNumber, out Card? card);
            if (card == null)
                TempData["ErrorMessage"] = "شماره کارت وارد شده موجود نیست";
            else
            {
                if (_cardAppService.Block(card))
                {
                    TempData["ErrorMessage"] = "حساب شما مسدود شد";

                }
                if (!result.Success)
                {
                    TempData["ErrorMessage"] = result.Message;

                }
                else
                {
                    if (!_cardAppService.PasswordIsValid(card, password))
                        TempData["ErrorMessage"] = "پسورد اشتباه است پس از سه بار تلاش ناموفق حساب شما مسدود خواهد شد";
                    else
                    {
                        HttpContext.Session.SetObject("card", card);
                        return RedirectToAction("Menu", "Card");
                    }
                }
            }
            return RedirectToAction("Login", "Card");
        }

        public IActionResult Menu()
        {
            var card = HttpContext.Session.GetObject<Card>("card");
            if(card == null)
            {
                return RedirectToAction("Index", "Card");
            }
            ViewData["Title"] = "منو";
            ViewBag.CssFile = "menu.css";
            return View();
        }
        public IActionResult ChangePassword()
        {
            var card = HttpContext.Session.GetObject<Card>("card");
            if (card == null)
            {
                return RedirectToAction("Index", "Card");
            }
            ViewData["Title"] = "تغییر رمز";
            ViewBag.CssFile = "login.css";
            return View();
        }
        [HttpPost]
        public IActionResult ChangeCardPassword(string newPassword, string repetedNewPassword)
        {
            var card = HttpContext.Session.GetObject<Card>("card");
            if (card == null)
            {
                return RedirectToAction("Index", "Card");
            }
            if(newPassword != repetedNewPassword)
            {
                TempData["ErrorMessage"] = "رمز جدید و تکرار آن مطابقت ندارند";
                return RedirectToAction("ChangePassword", "Card");
            }

            else
            {
                 var result = _cardAppService.ChangePassword(card, newPassword);
                if(!result.Success)
                {
                    TempData["ErrorMessage"] = result.Message;
                 
                }
                else
                {
                    TempData["SuccessMessage"] = result.Message;
                }
                return RedirectToAction("ChangePassword", "Card");
            }
        }

        public IActionResult GetInformation()
        {
            var card = HttpContext.Session.GetObject<Card>("card");
            if (card != null)
            {
                _cardAppService.GetBalance(card, out float balance);
                card.Balance = balance;
            }
            if (card == null)
            {
                return RedirectToAction("Index", "Card");
            }
            ViewData["Title"] = "موجودی";
            ViewBag.CssFile = "login.css";
            return View(card);

        }

    }
}
