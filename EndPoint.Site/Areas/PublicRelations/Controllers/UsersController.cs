using Application.Services.Location.Province;
using Application.Services.UserService;
using Azmoon.Application.Service.PublicRelations.Location.City;
using Azmoon.Application.Service.PublicRelations.Location.Province;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Common.Pagination;
using Azmoon.Domain.Entities;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Account;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Operator;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;



namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IGetProvinceService _getProvinceService;
        private readonly IChengeStatusUserService _chengeStatusUserService;
        private readonly IGetProvinceNameById _getProvinceNameById;
        private readonly IGetCityNameById _getCityNameById;
        private readonly IGetOperatorListServices _getOperatorListServices;
        private readonly IGetNormalizedNameByNameService _getNormalizedNameByNameService;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

        public UsersController(UserManager<User> userManager,
            IGetProvinceService getProvinceService,
            IChengeStatusUserService chengeStatusUserService,
            IGetProvinceNameById getProvinceNameById,
            IGetCityNameById getCityNameById,
            IGetOperatorListServices getOperatorListServices,
            IGetNormalizedNameByNameService getNormalizedNameByNameService,
            IGetNameByNormalizedNameService getNameByNormalizedNameService)
        {
            _userManager = userManager;
            _getProvinceService = getProvinceService;
            _chengeStatusUserService = chengeStatusUserService;
            _getProvinceNameById = getProvinceNameById;
            _getCityNameById = getCityNameById;
            _getOperatorListServices = getOperatorListServices;
            _getNormalizedNameByNameService = getNormalizedNameByNameService;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("UserPR");
            var userIds = usersInRole.Select(u => u.Id).ToList();
            var result = _userManager.Users.Where(u => userIds.Contains(u.Id)).AsQueryable();
            int rowCount = 0;


            var users = result
                //.OrderBy(p => p.InsertTime)
                .ToPaged(page, pageSize, out rowCount)
                       .Select(p => new PublicRelationUserListDto
                       {
                           Id = p.Id,
                           FullName = p.FirstName + " " + p.LastName,
                           PersonnelCode = p.UserName,
                           Province = p.Province,
                           PhoneNumber = p.PhoneNumber,
                           IsActive = p.IsActive,
                           Operator = _getNameByNormalizedNameService.Execute(p.Operator).Data,
                       }).ToList();

            // اطلاعات مربوط به صفحه‌بندی را به ویو منتقل می‌کنیم
            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRowCount = rowCount;


            return View(users);
        }



        public IActionResult PartialViewResetPasswordModal(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            return PartialView(new PublicRelationResetPasswordDto
            {
                UserId = UserId,
                FullName = user.FirstName + " " + user.LastName,
                IsActive = user.IsActive
            });
        }

        [HttpPost]
        public async Task<IActionResult> PartialViewResetPasswordModal(PublicRelationResetPasswordDto reset)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(reset);
            }

            if (reset.Password != reset.ConfirmPassword)
            {
                return BadRequest(new { isSuccess = false, message = "رمز عبور با تکرار آن برابر نیست" });
            }

            var user = await _userManager.FindByIdAsync(reset.UserId);

            if (user == null)
            {
                return BadRequest(new { isSuccess = false, message = "کاربر مورد نظر یافت نشد." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, reset.Password);

            if (result.Succeeded)
            {
                return Ok(new { isSuccess = true, message = "رمز عبور با موفقیت تغییر یافت." });
            }
            else
            {
                return BadRequest(new { isSuccess = false, message = "خطا در تغییر رمز عبور.", errors = result.Errors });
            }
        }

        public IActionResult PartialViewEditUserModal(string UserId)
        {
            ViewBag.Province = new SelectList(_getProvinceService.Execute().Data, "Id", "Name");


            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            GetUserEditDto userEdit = new GetUserEditDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Province = user.Province,
                City = user.City,
                Operator = user.Operator,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
          
            };
            return PartialView(userEdit);
        }

        [HttpPost]
        public IActionResult PartialViewEditUserModal(PostUserEditDto editDto)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByIdAsync(editDto.Id).Result;

                // Check if there are any changes
                if (IsUserUnchanged(user, editDto))
                {
                    return Json(new { isSuccess = true, message = "شما هیچ تغییری انجام نداده‌اید." });
                }

                // Update user properties
                user.FirstName = editDto.FirstName;
                user.LastName = editDto.LastName;
                user.UserName = editDto.UserName;

                if (editDto.Province != null)
                {
                    var provinceName = _getProvinceNameById.Execute(editDto.Province);
                    user.Province = provinceName;
                }
                else
                {
                    user.Province = user.Province;
                }

                if (editDto.City != null)
                {
                    var cityName = _getCityNameById.Execute(editDto.City);
                    user.City = cityName;
                }
                else
                {
                    user.City = user.City;
                }

                user.PhoneNumber = editDto.PhoneNumber;
                

                // Update the user
                var result = _userManager.UpdateAsync(user).Result;

                if (result.Succeeded)
                {
                    return Json(new { isSuccess = true, message = "ویرایش با موفقیت انجام شد." });
                }

                // Handle update errors
                string message = "";
                foreach (var item in result.Errors.ToList())
                {
                    message += item.Description + Environment.NewLine;
                }
                TempData["Message"] = message;
                return Json(new { isSuccess = false, message = "خطایی رخ داده است" });
            }

            // Model state is not valid
            return Json(new { isSuccess = false, message = "خطایی در اعتبارسنجی داده‌ها رخ داده است" });
        }

        // Helper method to check if the user object is unchanged
        private bool IsUserUnchanged(User user, PostUserEditDto editDto)
        {
            return user.FirstName == editDto.FirstName &&
                   user.LastName == editDto.LastName &&
                   user.UserName == editDto.UserName &&
                   user.Province == (_getProvinceNameById.Execute(editDto.Province) ?? user.Province) &&
                   user.City == (_getCityNameById.Execute(editDto.City) ?? user.City) &&
                   user.PhoneNumber == editDto.PhoneNumber;
        }

        public IActionResult UserSatusChange(string UserId)
        {
            return Json(_chengeStatusUserService.Execute(UserId));
        }

        public IActionResult PartialViewDeleteUserModal(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            GetUserDeleteDto userDelete = new GetUserDeleteDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PersonnelCode = user.UserName,
                IsActive = user.IsActive,
            };
            return PartialView(userDelete);
        }

        [HttpPost]
        public IActionResult PartialViewDeleteUserModal(GetUserDeleteDto userDelete)
        {

            var user = _userManager.FindByIdAsync(userDelete.Id).Result;
            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return Json(new { isSuccess = true, message = $"" + user.FirstName + " " + user.LastName + " " + "با موفقیت انجام شد" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return Json(new { isSuccess = false, message = "خطایی رخ داده است" });

        }

        public IActionResult PartialViewDetailUserModal(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            GetUserDetailDto userDetail = new GetUserDetailDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Province = user.Province,
                City = user.City,
                Operator = user.Operator,
                PersonnelCode = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
              
            };
            return PartialView(userDetail);
        }

        public IActionResult PartialViewTemporaryRoleUser(string UserId)
        {
            ViewBag.TemporaryRole = new SelectList(_getOperatorListServices.Execute().Data, "Id", "Name");

            var user = _userManager.FindByIdAsync(UserId).Result;
            var operatorName= _getNameByNormalizedNameService.Execute(user.Operator).Data;


            GetUserDetailDto userDetail = new GetUserDetailDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Province = user.Province,
                City = user.City,
                Operator = operatorName,
                PersonnelCode = user.UserName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
          
            };
            return PartialView(userDetail);

        }

        [HttpPost]
        public IActionResult PartialViewTemporaryRoleUser(PostOperatorEditDto postOperatorEditDto)
        {

            if (ModelState.IsValid)
            {
                var user = _userManager.FindByIdAsync(postOperatorEditDto.UserId).Result;

                // Check if there are any changes
                //if (IsUserUnchanged(user, postOperatorEditDto))
                //{
                //    return Json(new { isSuccess = true, message = "شما هیچ تغییری انجام نداده‌اید." });
                //}

                // Update user properties
                var normalizedName = _getNormalizedNameByNameService.Execute(postOperatorEditDto.OperatorName).Data;
                user.Operator = normalizedName;



               

                // Update the user
                var result = _userManager.UpdateAsync(user).Result;

                if (result.Succeeded)
                {
                    return Json(new { isSuccess = true, message = "ویرایش با موفقیت انجام شد." });
                }

                // Handle update errors
                string message = "";
                foreach (var item in result.Errors.ToList())
                {
                    message += item.Description + Environment.NewLine;
                }
                TempData["Message"] = message;
                return Json(new { isSuccess = false, message = "خطایی رخ داده است" });
            }

            // Model state is not valid
            return Json(new { isSuccess = false, message = "خطایی در اعتبارسنجی داده‌ها رخ داده است" });
        }

        public IActionResult AddUser()
        {
            ViewBag.Province = new SelectList(_getProvinceService.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(PublicRelationRegisterDto register)
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
                IsActive = false,

            };

            var result = _userManager.CreateAsync(newUser, register.Password).Result;
            if (result.Succeeded)
            {
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
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return Json(register);
        }





    }
}
