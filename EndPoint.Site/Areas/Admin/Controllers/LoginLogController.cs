using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Login.Command;
using Azmoon.Application.Service.WorkPlace.Query;
using DocumentFormat.OpenXml.Wordprocessing;
using EndPoint.Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace EndPoint.Site.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class LoginLogController : Controller
    {
        private readonly ILoginCRUD _loginService;
        private readonly IGetWorkplacFirstToEndParent _workplacFirstToEndParent;
        private readonly IDataBaseContext _context;
        public LoginLogController(ILoginCRUD loginService, 
            IGetWorkplacFirstToEndParent workplacFirstToEndParent,
            IDataBaseContext context)
            {
            _loginService = loginService;
            _workplacFirstToEndParent = workplacFirstToEndParent;
            _context = context;
            }
        public async Task<IActionResult> Index(string searchIp, string searchUserName, int page = 1)
            {
            int pageSize = 10;
            if (searchIp == "clear")
                {
                return RedirectToAction("Index");
                }
            var logsQuery = _loginService.GetAllAsync().Result.AsQueryable();

            // اعمال فیلترهای جستجو در صورت موجود بودن
            if (!string.IsNullOrWhiteSpace(searchIp))
                {
                logsQuery = logsQuery.Where(l => l.Ip.Contains(searchIp));
                }

            if (!string.IsNullOrWhiteSpace(searchUserName))
                {
                logsQuery = logsQuery.Where(l => l.UserName.Contains(searchUserName));
                }
            ViewBag.SearchIp = searchIp;
            ViewBag.SearchUserName = searchUserName;

            var pagedLogs = logsQuery
                 .OrderByDescending(l => l.RegesterAt)
                 .Select(p => new UserShowPersonDtoLogin()
                     {
                     Ip = p.Ip,
                     UserName = p.UserName,
                     RegesterAt = p.RegesterAt,
                     }).AsQueryable();


            var pagedUserLogs = await pagedLogs.ToPagedListAsync(page, pageSize);

            for (int i = 0; i < pagedUserLogs.Count(); i++)
                {
                var user = _context.Users.AsNoTracking().Where(p => p.UserName == pagedUserLogs.ElementAt(i).UserName).FirstOrDefault();
                pagedUserLogs.ElementAt(i).WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent(user.WorkPlaceId).Data;
                pagedUserLogs.ElementAt(i).FirstName = user.FirstName;
                pagedUserLogs.ElementAt(i).LastName = user.LastName;
                pagedUserLogs.ElementAt(i).Phone = user.Phone;
                }
       


            return View(pagedUserLogs);
            }

        }
    }
