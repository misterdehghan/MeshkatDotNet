using Application.Services.Location.Province;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.Facad;
using Azmoon.Application.Interfaces.User;
using Azmoon.Application.Interfaces.WorkPlace;
using Azmoon.Application.Service.Login.Command;
using Azmoon.Application.Service.PublicRelations.CAPTCHA;
using Azmoon.Application.Service.PublicRelations.Location.City;
using Azmoon.Application.Service.PublicRelations.Location.Province;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Application.Service.User.Dto;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities;
using DNTCaptcha.Core;
using DocumentFormat.OpenXml.InkML;
using DotNet.RateLimiter.ActionFilters;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Account;
using EndPoint.Site.Models;
using EndPoint.Site.Useful.Static;
using EndPoint.Site.Useful.Ultimite;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace EndPoint.Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDataBaseContext _context;  
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGetWorkPlace _getWorkPlace;
        private readonly IUserFacad _userFacad;
        private readonly ICaptchaService _captchaService;
        private readonly IGetProvinceService _getProvinceService;
        private readonly IGetCityService _getCityService;
        private readonly IGetProvinceNameById _getProvinceNameById;
        private readonly IGetCityNameById _getCityNameById;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;
        private readonly ILoginCRUD _loginCRUD;
        private static readonly NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();
        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
             IUserFacad userFacad,
             IRegisterUser registerUser,
             IGetWorkPlace getWorkPlace,
             ICaptchaService captchaService,
             IGetProvinceService getProvinceService,
             IGetCityService getCityService,
             IGetProvinceNameById getProvinceNameById,
             IGetCityNameById getCityNameById,
             IGetNameByNormalizedNameService getNameByNormalizedNameService,
             IDataBaseContext context,
             ILoginCRUD loginCRUD)
            {
            _userManager = userManager;
            _signInManager = signInManager;
            _userFacad = userFacad;
            _getWorkPlace = getWorkPlace;
            _captchaService = captchaService;
            _getProvinceService = getProvinceService;
            _getCityService = getCityService;
            _getProvinceNameById = getProvinceNameById;
            _getCityNameById = getCityNameById;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
            _context = context;
            _loginCRUD = loginCRUD;
            }

        [HttpGet]
        [RateLimit(PeriodInSec = 300, Limit = 10)]
        public IActionResult Login(string returnUrl = "/")
        {
            nlog.Trace("Trace");
            return View(new LoginDto
            {
                ReturnUrl = returnUrl,
                // ReturnUrl = "/",
            });
        }

        [HttpPost]
        [RateLimit(PeriodInSec = 30, Limit = 10)]
        [ValidateAntiForgeryToken]
        //[ValidateDNTCaptcha(
        //    ErrorMessage = "عبارت امنیتی را به درستی وارد نمائید",
        //    CaptchaGeneratorLanguage = Language.Persian,
        //    CaptchaGeneratorDisplayMode = DisplayMode.NumberToWord)]
        public async Task<IActionResult> Login(LoginDto login, string CaptchaInputText = "")
        {
            await _signInManager.SignOutAsync();
            nlog.Trace("Trace");
            //  string CaptchaInputTextHASH = Ultimite.EncodePasswordMd5(CaptchaInputText);
            //var Captcha=  HttpContext.Session.GetString("Captcha");
            //  HttpContext.Session.Remove("Captcha");
            //  if (Captcha == null || Captcha.ToString() != CaptchaInputTextHASH)
            //  {
            //      ModelState.AddModelError(string.Empty, "عبارت امنیتی اشتباه است!!!");
            //      return View(login);
            //  }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "اطلاعات وارد شده نامعتبر می باشد !!!");
                return View(login);
            }
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            // اگر هدر وجود ندارد، از IP مستقیم استفاده کن
            if (string.IsNullOrEmpty(ip))
                {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                }

            // حذف پورت از IP (در صورت وجود)
            if (!string.IsNullOrEmpty(ip) && ip.Contains(":"))
                {
                ip = ip.Split(':')[0];
                }

            //var testIP = "10,34,99,9";
            HtmlSanitizer sanitizer = new HtmlSanitizer();

            login.UserName = sanitizer.Sanitize(login.UserName);
            login.Password = sanitizer.Sanitize(login.Password);
        
            var user =await _userManager.FindByNameAsync(login.UserName);

    
            if (user != null)
            {
                var result = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true).Result;
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "حساب کاربری شما غیر فعال گردیده است شما می توانید بعد از 20 دقیقه مجددا وارد شوید!!!");
                    return View();
                }


                if (result.Succeeded == true)
                {
                    var logObj = new Azmoon.Domain.Entities.LoginLog()
                        {
                        Ip = ip,
                        UserName = login.UserName,
                        Status = 1
                        };
                   await _loginCRUD.CreateAsync(logObj);
                    if (user.NumberBankAccunt == null || user.NumberBankAccunt.ToString().Length < 13)
                    {
                        return RedirectToAction("UpdateAccunt", new { userName = login.UserName, pass = login.Password });
                    }
                    var role = _userManager.GetRolesAsync(user).Result;

                    if (login.ReturnUrl != "/" && login.ReturnUrl != "/Student/Quizzes/Start")
                    {
                        return Redirect(login.ReturnUrl);
                    }
                    else
                    {

                        if (role.Count < 2 && role.Contains("User"))
                        {
                            return Redirect("/Student/Quizzes/StudentActiveEventsAll");
                        }
                        else
                        {
                            return Redirect("/Admin/Apnl/Index");
                        }

                    }



                }
                else
                {
                    ModelState.AddModelError(string.Empty, "نام کاربری و یا رمز عبور را به درستی وارد نمائید !!!");
                }

            }
            ModelState.AddModelError(string.Empty, "اطلاعات وارد شده نامعتبر می باشد !!!");
            return View(login);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            nlog.Trace("Trace");
            return View();
        }
        [RateLimit(PeriodInSec = 30, Limit = 10)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        //[ValidateDNTCaptcha(
        //    ErrorMessage = "عبارت امنیتی را به درستی وارد نمائید",
        //    CaptchaGeneratorLanguage = Language.Persian,
        //    CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbersToWords)]
        public IActionResult Register(ShortRegisterDto dto)
        {
            ModelState.Remove("Id");
            var validate_meki_code = dto.melli.NationalCodeValidation();
            if (!validate_meki_code.IsSuccess)
            {
                ModelState.AddModelError("melli", validate_meki_code.Message);
            }
            if (!ModelState.IsValid)
            {
                var query = from state in ModelState.Values
                            from error in state.Errors
                            select error.ErrorMessage;

                var errorList = query.ToList();
                ViewBag.Errors = errorList;
                //var workPlaceSelectListItem = _workPlaceFacad.GetWorkPlaceSelectListItem.Exequte(null).Data;
                //TempData["workPlaceSelectListItem"] = workPlaceSelectListItem;
                return View();
            }
            else
            {
                var model = _userFacad.RegisterUser.registerExqute(dto);
                if (!model.IsSuccess)
                {
                    if (model.Data == 1)
                    {
                        ModelState.AddModelError("کاربر موجود بود", model.Message);
                    }
                    if (model.Data == 0)
                    {
                        ModelState.AddModelError("کاربری در دیتابیس اولیه نبود", model.Message);
                        return RedirectToAction("CreateUser", dto);
                    }

                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    ViewBag.Errors = errorList;
                    //var workPlaceSelectListItem = _workPlaceFacad.GetWorkPlaceSelectListItem.Exequte(null).Data;
                    //TempData["workPlaceSelectListItem"] = workPlaceSelectListItem;
                    return View();
                }

                if (model.IsSuccess)
                {
                    var user = _userManager.FindByNameAsync(dto.personeli.ToString()).Result;
                    var SignOuted = _signInManager.SignOutAsync().IsCompleted;
                    if (SignOuted)
                    {
                        var signIned = _signInManager.PasswordSignInAsync(user, dto.Password, true
                                     , true).Result;
                    }


                    return RedirectToAction("Index", "Home");

                }
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult GetWorkPlaceTreeView(string name, string family)
        {
            var model = _getWorkPlace.Execute(null);
            var viewHtml = this.RenderViewAsync("_PartialWorkPlaceTreeView", model.Data, true);
            return Json(new ResultDto<string>
            {
                Data = viewHtml,
                IsSuccess = true,
                Message = "موفق"
            });
        }

        public IActionResult GetDarajeh(int type)
        {
            if (type == 1)
            {
                var model = StaticList.listObjDarajeh();
                return Json(new ResultDto<Listoption>
                {
                    Data = model,
                    IsSuccess = true,
                    Message = "موفق"
                });
            }
            if (type == 0)
            {
                var model = StaticList.listObjRotbeh();
                return Json(new ResultDto<Listoption>
                {
                    Data = model,
                    IsSuccess = true,
                    Message = "موفق"
                });
            }
            if (type == 2)
            {
                var model = StaticList.listObjRotbehRoohani();
                return Json(new ResultDto<Listoption>
                {
                    Data = model,
                    IsSuccess = true,
                    Message = "موفق"
                });
            }


            return Json(new ResultDto<Listoption>
            {
                Data = null,
                IsSuccess = false,
                Message = "ناموفق"
            });
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            nlog.Trace("Trace");
            // ViewData["Darajeh"] = StaticList.listeDarajeh;
            ViewData["listTypeDarajeh"] = StaticList.listTypeDarajeh;
            ViewData["darajah"] = StaticList.listObjAllDarajeh();
            return View(null);
        }

        [HttpPost]
        [RateLimit(PeriodInSec = 30, Limit = 10)]
        [ValidateAntiForgeryToken]
        //[ValidateDNTCaptcha(
        //    ErrorMessage = "عبارت امنیتی را به درستی وارد نمائید",
        //    CaptchaGeneratorLanguage = Language.Persian,
        //    CaptchaGeneratorDisplayMode = DisplayMode.NumberToWord)]
        public IActionResult CreateUser(IFormFile Image, RegisterUserDto dto)
        {
            nlog.Trace("Trace");
            ModelState.Remove("Id");
            if (dto.WorkPlaceId == 0)
            {
                ModelState.AddModelError("WorkPlaceId", "محل خدمت را وارد نمائید!!");
            }
            if (dto.darajeh == 0)
            {
                ModelState.AddModelError("darajeh", "درجه را انتخاب نمائید!!");
            }
            if (!ModelState.IsValid)
            {
                var query = from state in ModelState.Values
                            from error in state.Errors
                            select error.ErrorMessage;

                var errorList = query.ToList();
                errorList.Remove("The value '' is invalid.");
                ViewBag.Errors = errorList;
                ViewData["darajah"] = StaticList.listObjAllDarajeh();
                ViewData["listTypeDarajeh"] = StaticList.listTypeDarajeh;
                return View(dto);
            }
            else
            {
                var model = _userFacad.CreateUser.Register(dto, Image);
                if (!model.IsSuccess)
                {

                    ModelState.AddModelError("کاربر موجود بود", model.Message);
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    ViewBag.Errors = errorList;
                    ViewData["listTypeDarajeh"] = StaticList.listTypeDarajeh;
                    return View();
                }

                if (model.IsSuccess)
                {
                    var user = _userManager.FindByNameAsync(dto.personeli.ToString()).Result;

                    //این کد موقت بوده و برای سامانه بصی است و برای افزودن نقش به کاربران ان سامانه می باشد

                     _userManager.AddToRoleAsync(user, "TrafficUser");

                    //***************************************************


                    var SignOuted = _signInManager.SignOutAsync().IsCompleted;
                    if (SignOuted)
                    {
                        var signIned = _signInManager.PasswordSignInAsync(user, dto.Password, true
                                     , true).Result;
                    }


                    return RedirectToAction("Index", "Home");

                }
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        [RateLimit(PeriodInSec = 30, Limit = 10)]
        public IActionResult ForgotPassword()
        {

            nlog.Trace("Trace");

            return View(null);
        }

        [RateLimit(PeriodInSec = 30, Limit = 10)]
        [ValidateAntiForgeryToken]
        // [ValidateDNTCaptcha(
        //ErrorMessage = "عبارت امنیتی را به درستی وارد نمائید",
        //CaptchaGeneratorLanguage = Language.Persian,
        //CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbersToWords)]
        public IActionResult ForgotPassword(ForgotPasswordDto dto, string CaptchaInputText)
        {
            nlog.Trace("Trace");
            string CaptchaInputTextHASH = Ultimite.EncodePasswordMd5(CaptchaInputText);
            var Captcha = HttpContext.Session.GetString("Captcha");
            HttpContext.Session.Remove("Captcha");
            if (Captcha == null || Captcha.ToString() != CaptchaInputTextHASH)
            {
                return Json(new ResultDto<string>
                {
                    Data = "",
                    IsSuccess = false,
                    Message = "عبارت امنیتی اشتباه است!!!"
                });
            }
            if (ModelState.IsValid)
            {
                var model = _userFacad.forgotPassword.forgotPassword(dto);
                if (model.IsSuccess)
                {
                    var viewHtml = this.RenderViewAsync("_PartialChangePassword", model, true);
                    return Json(new ResultDto<string>
                    {
                        Data = viewHtml,
                        IsSuccess = true,
                        Message = "موفق"
                    });
                }

                return Json(new ResultDto<string>
                {
                    Data = "",
                    IsSuccess = false,
                    Message = model.Message
                });
            }
            string messages = string.Join("\n ", ModelState.Values
                                      .SelectMany(x => x.Errors)
                                      .Select(x => x.ErrorMessage));
            return Json(new ResultDto<string>
            {
                Data = "",
                IsSuccess = false,
                Message = messages
            });
        }
        public IActionResult ResetPassword(RessetPassInForgotPassdto dto)
        {
            nlog.Trace("Trace");
            if (ModelState.IsValid)
            {

                var user = _userManager.FindByIdAsync(dto.userId).Result;
                _signInManager.SignOutAsync();

                if (user != null)
                {
                    var result2 = _userManager.RemovePasswordAsync(user).Result;
                    if (result2.Succeeded)
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, dto.Password);
                        var result3 = _userManager.UpdateAsync(user).Result;

                        if (result3.Succeeded)
                        {
                            //این کد موقت بوده و برای سامانه بصی است و برای افزودن نقش به کاربران ان سامانه می باشد

                            _userManager.AddToRoleAsync(user, "TrafficUser");

                            //***************************************************

                            var result = _signInManager.PasswordSignInAsync(user, dto.Password, false, true).Result;
                            if (result.Succeeded == true)
                            {
                                return Json(new ResultDto<string>
                                {
                                    Data = "/Home/Index",
                                    IsSuccess = true,
                                    Message = "موفق"
                                });
                            }
                            return Json(new ResultDto<string>
                            {
                                Data = "/Home/Index",
                                IsSuccess = true,
                                Message = "موفق"
                            });
                        }
                    }
                }
                return Json(new ResultDto<string>
                {
                    Data = "",
                    IsSuccess = false,
                    Message = "کاربر یافت نشد!!!"
                });

            }

            string messages = string.Join("\n ", ModelState.Values
                                             .SelectMany(x => x.Errors)
                                             .Select(x => x.ErrorMessage));

            return Json(new ResultDto<string>
            {
                Data = messages,
                IsSuccess = false,
                Message = messages
            });
        }

        public IActionResult GetHasAcount(long personeli, string melicode)
        {
            var model = _userFacad.CreateUser.HassUser(personeli, melicode);

            return Json(model);
        }

        public IActionResult AccessDenied(string message, string access)
        {
            //   var arguments = (int x, string y) => { return (x, y) };

            if (!String.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            if (!String.IsNullOrEmpty(access))
            {
                ViewBag.access = access;
            }
            return View();
        }

        public IActionResult UpdateAccunt(string userName, string pass)
        {


            var user = _userManager.FindByNameAsync(userName).Result;
            var result = _signInManager.PasswordSignInAsync(user, pass, false, true).Result;
            if (result.Succeeded == true)
            {
                if (user.NumberBankAccunt == null || user.NumberBankAccunt.ToString().Length < 13)
                {
                    ViewBag.username = userName;
                    ViewBag.pass = pass;
                    _signInManager.SignOutAsync();
                    return View();
                }
            }
            return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult UpdateAccunt(long hesab, string userName, string pass)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
            var result = _signInManager.PasswordSignInAsync(user, pass, false, true).Result;
            if (result.Succeeded == true)
            {

                if (hesab.ToString().Length >= 13 && hesab.ToString().Length < 16)
                {
                    user.NumberBankAccunt = hesab;
                    var result3 = _userManager.UpdateAsync(user).Result;
                    if (result3.Succeeded)
                    {
                        return Redirect("/Home/Index");
                    }
                }
                _signInManager.SignOutAsync();
                List<string> Errors = new List<string>();
                Errors.Add("تعداد اعداد شماره حساب وارد شده برابر فرمت عنوان شده نمی باشد ");
                ViewBag.username = userName;
                ViewBag.pass = pass;
                ViewBag.Errors = Errors;
                return View();
            }

            List<string> Errors2 = new List<string>();
            Errors2.Add("اطلاعات کاربر جعلی می باشد ");
            ViewBag.Errors = Errors2;
            return View();
        }

        [HttpGet]
        public IActionResult GetCitiesByProvinceId(int provinceId)
        {
            ViewBag.CityList = new SelectList(_getCityService.Execute(provinceId).Data, "Id", "NameCity");
            return Json(ViewBag.CityList);
        }

        [HttpGet]
        public IActionResult PublicRelationRegister()
        {
            ViewBag.Province = new SelectList(_getProvinceService.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult PublicRelationRegister(PublicRelationRegisterDto register)
        {
            if (ModelState.IsValid == false)
            {
                return View(register);
            }

            var randomEmail = Guid.NewGuid().ToString() + "@example.com";
            var provinceName = _getProvinceNameById.Execute(register.Province);
            var cityName = _getCityNameById.Execute(register.City);
            User newUser = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.PersonnelCode, // Use PersonnelCode as username
                Province = provinceName,
                City = cityName,
                PhoneNumber = register.PhoneNumber,
                Email = randomEmail, // A fake email has been assigned to the user
                IsActive = false
            };

            var result = _userManager.CreateAsync(newUser, register.Password).Result;
            if (!result.Succeeded)
            {
                // جمع کردن خطاها
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                var fullMessage = string.Join(" - ", errorMessages);

                // برگردوندن به صورت JSON برای نمایش در فرانت
                return Json(new { isSuccess = false, message = fullMessage });
            }

       
                // افزودن نقش "UserPR" به کاربر جدید
                var roleResult = _userManager.AddToRoleAsync(newUser, "UserPR").Result;
                if (!roleResult.Succeeded)
                {
                    // اگر افزودن نقش با خطا مواجه شد
                    string roleErrors = string.Join("، ", roleResult.Errors.Select(e => e.Description));
                    return Json(new { isSuccess = false, message = $"خطا در اختصاص نقش: {roleErrors}" });
                }

                return Json(new { isSuccess = true, message = "ثبت نام با موفقیت انجام شد." });

            
         
        }


        [HttpGet]
        [RateLimit(PeriodInSec = 300, Limit = 10)]
        public IActionResult PublicRelationLogin()
        {
            if (User.Identity.IsAuthenticated)
                {
                return Redirect("/PublicRelations/Dashboard/index");
                }
            return View();
        }

        [HttpPost]
        [RateLimit(PeriodInSec = 30, Limit = 10)]
        public IActionResult PublicRelationLogin(PublicRelationLoginDto login, string captcha)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            // بررسی کد CAPTCHA
            var storedCaptcha = HttpContext.Session.GetString("CaptchaCode");
            if (string.IsNullOrEmpty(storedCaptcha))
            {
                return Json(new { isSuccess = false, message = "کد کپچا منقضی شده است. لطفاً دوباره تلاش کنید." });
            }

            if (!string.Equals(captcha, storedCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                return Json(new { isSuccess = false, message = "کد کپچا اشتباه است." });
            }

            // اگر کد کپچا صحیح بود، حذف از سشن
            HttpContext.Session.Remove("CaptchaCode");

            // یافتن کاربر
            var user = _userManager.FindByNameAsync(login.UserName).Result;
            if (user == null || user.IsActive == false)
            {
                ModelState.AddModelError(string.Empty, "ورود ناموفق");
                return Json(new { isSuccess = false, message = "حساب کاربری شما فعال نیست" });
            }

            // گرفتن نقش‌های کاربر
            var roles = _userManager.GetRolesAsync(user).Result;

            // بررسی اینکه کاربر نقش UserPR داشته باشد
            if (!roles.Contains("UserPR"))
            {
                return Json(new { isSuccess = false, message = "شما مجاز به ورود به این سامانه نیستید." });
            }

            // تلاش برای ورود
            _signInManager.SignOutAsync();
            var result = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true).Result;

            if (result.Succeeded)
            {
                var welcomeMessage = "همکار گرامی " + user.FirstName + " " + user.LastName;
                return Json(new { isSuccess = true, redirectUrl = "/PublicRelations/Dashboard/index", message = welcomeMessage });
            }

            // اگر ورود موفقیت‌آمیز نبود
            ModelState.AddModelError(string.Empty, "ورود ناموفق");
            return Json(new { isSuccess = false, message = "نام کاربری یا رمز عبور اشتباه است." });
        }


        public IActionResult ShowCaptcha()
        {
            // تولید کد CAPTCHA به صورت تصادفی
            string captchaCode = _captchaService.GenerateCaptchaCode();

            // ذخیره کد CAPTCHA در سشن
            HttpContext.Session.SetString("CaptchaCode", captchaCode);

            // ایجاد حافظه برای تصویر CAPTCHA
            var memoryStream = new MemoryStream();

            // تولید تصویر CAPTCHA و ذخیره آن در حافظه
            _captchaService.GenerateCaptchaImage(captchaCode, memoryStream);

            // بازگشت به ابتدا برای ارسال محتوا
            memoryStream.Seek(0, SeekOrigin.Begin);

            // ارسال تصویر PNG به مرورگر
            return File(memoryStream, "image/png");
        }

        [HttpPost]
        public IActionResult VerifyCaptcha(string captcha)
        {
            // گرفتن کد ذخیره‌شده در سشن
            var storedCaptcha = HttpContext.Session.GetString("CaptchaCode");

            if (string.IsNullOrEmpty(storedCaptcha))
            {
                ModelState.AddModelError("", "کد کپچا منقضی شده است. لطفاً دوباره تلاش کنید.");
                return View();
            }

            // مقایسه کد وارد شده با کد ذخیره‌شده
            if (captcha == storedCaptcha)
            {
                HttpContext.Session.Remove("CaptchaCode"); // حذف کد از سشن
                return Content("کد کپچا صحیح است!");
            }
            else
            {
                ModelState.AddModelError("", "کد کپچا نادرست است.");
                return View();
            }
        }

    }
}
