
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Application.Services.Location.Province;

using Microsoft.EntityFrameworkCore;
using Azmoon.Domain.Entities;
using Azmoon.Application.Service.PublicRelations.OperatorServices;


namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

        private static readonly NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();


        public AccountController(UserManager<User> userManager,

            SignInManager<User> signInManager,
            IGetProvinceNameById getProvinceNameById,
            IGetNameByNormalizedNameService getNameByNormalizedNameService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;

        }


        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("PublicRelationLogin", "Account", new { area = "" });

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Profile()
        {
            // گرفتن اطلاعات کاربری که لاگین کرده
            var user = _userManager.GetUserAsync(User).Result;
            var operatorName = _getNameByNormalizedNameService.Execute(user.Operator).Data;
            ViewBag.OperatorName = operatorName;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // ارسال اطلاعات کاربر به ویو
            return View(user);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}