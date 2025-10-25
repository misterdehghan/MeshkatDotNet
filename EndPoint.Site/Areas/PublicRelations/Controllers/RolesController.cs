using Azmoon.Common.Pagination;
using Azmoon.Domain.Entities;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    [Authorize(Roles = "SuperUserAccountPR")]
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var result = _roleManager.Roles.Where(r => r.Name.EndsWith("PR")).AsQueryable();
            int rowCount = 0;


            var role = result
                .ToPaged(page, pageSize, out rowCount)
                .Select(p => new RoleListDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();

            // اطلاعات مربوط به صفحه‌بندی را به ویو منتقل می‌کنیم
            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRowCount = rowCount;
            return View(role);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddNewRoleDto newRoleDto)
        {
            if (string.IsNullOrEmpty(newRoleDto.Name))
            {
                ModelState.AddModelError("", "Role name is required.");
                return View();
            }

            // استفاده از RoleManager برای ایجاد نقش
            var role = new Role(newRoleDto.Name);
            var result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Role '{newRoleDto.Name}' created successfully.";
                return RedirectToAction("Index"); // لیست نقش‌ها
            }

            // نمایش خطاهای مربوط به Identity
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }


        public IActionResult Edit(string Id)
        {
            var role = _roleManager.FindByIdAsync(Id).Result;
            RoleEditDto roleEdit = new RoleEditDto()
            {
                Name = role.Name,

            };
            return View(roleEdit);
        }
        [HttpPost]
        public IActionResult Edit(RoleEditDto roleEdit)
        {
            var role = _roleManager.FindByIdAsync(roleEdit.Id).Result;
            role.Name = roleEdit.Name;
            var result = _roleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Roles", new { area = "Admin" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(roleEdit);
        }


        public IActionResult Delete(string Id)
        {
            var role = _roleManager.FindByIdAsync(Id).Result;
            RoleDeleteDto roleDelete = new RoleDeleteDto()
            {
                Id = role.Id,
                Name = role.Name,
            };
            return View(roleDelete);
        }
        [HttpPost]
        public IActionResult Delete(RoleDeleteDto roleDelete)
        {
            var role = _roleManager.FindByIdAsync(roleDelete.Id).Result;
            var result = _roleManager.DeleteAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Roles", new { area = "Admin" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(roleDelete);
        }


        public async Task<IActionResult> PartialViewAssignRoleToUser(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // دریافت نقش فعلی کاربر
            var userRoles = await _userManager.GetRolesAsync(user);
            var prRoles = userRoles.Where(role => role.EndsWith("PR")).ToList();

            // دریافت تمام نقش‌ها
            var roles = _roleManager.Roles
                .Where(r => r.Name.EndsWith("PR")).ToList();

            var userDetail = new GetAssignRoleToUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Operator = user.Operator,
                PersonnelCode = user.UserName,
                Roles = roles, // ارسال لیست نقش‌ها
                CurrentRole = prRoles // ارسال نقش فعلی
            };

            return PartialView(userDetail);
        }


        [HttpPost]
        public JsonResult AssignRoleToUserAjax([FromBody] AssignRoleRequest model)
        {
            var user = _userManager.FindByIdAsync(model.UserId).Result;
            if (user == null)
            {
                return Json(new { success = false, message = "کاربر یافت نشد" });
            }

            var userRoles = _userManager.GetRolesAsync(user).Result;
        

            var roleExists = _roleManager.RoleExistsAsync(model.RoleName).Result;
            if (!roleExists)
            {
                return Json(new { success = false, message = "نقش یافت نشد" });
            }

            var result = _userManager.AddToRoleAsync(user, model.RoleName).Result;
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "نقش با موفقیت به کاربر اختصاص داده شد" });
            }

            return Json(new { success = false, message = "خطا در اختصاص نقش" });
        }

        public class AssignRoleRequest
        {
            public string UserId { get; set; }
            public string RoleName { get; set; }
        }


        [HttpPost]
        public IActionResult RemoveRoleFromUser(string userId, string roleName)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return Json(new { success = false, message = "کاربر یافت نشد." });

            var isInRole = _userManager.IsInRoleAsync(user, roleName).Result;
            if (!isInRole)
                return Json(new { success = false, message = "کاربر این نقش را ندارد." });

            var result = _userManager.RemoveFromRoleAsync(user, roleName).Result;
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "نقش با موفقیت حذف شد." });
            }


            return Json(new { success = false, message = "خطا در حذف نقش." });
        }

        //[HttpGet]
        //public IActionResult RemoveRoleFromUser(string userId, string roleName)
        //{
        //    return Content($"UserID: {userId}, RoleName: {roleName}");
        //}




    }
}
