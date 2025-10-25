using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.WorkPlace;
using Azmoon.Application.Service.WorkPlace.Dto;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Domain.Entities;
using Azmoon.Domain.Entities.Template;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using EndPoint.Site.Models;
using EndPoint.Site.Useful.Ultimite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;
using static EndPoint.Site.Models.ModelValidations;



namespace EndPoint.Site.Areas.Person.Controllers
    {
    [Area("Person")]
    [Authorize]
    [Authorize(Roles = "Administrator ,TrafficManager ,TrafficUser")]
    public class HourlyLeaveController : Controller
        {
        private readonly IDataBaseContext _context;
        private readonly IGetWorkplacFirstToEndParent _workplacFirstToEndParent;
        private readonly IGetWorkPlace _getWorkPlace;
        private readonly IGetChildrenWorkPlace _childrenWorkPlace;

        public HourlyLeaveController(IDataBaseContext context,
            IGetWorkplacFirstToEndParent workplacFirstToEndParent,
            IGetWorkPlace getWorkPlace, IGetChildrenWorkPlace childrenWorkPlace)
            {
            _context = context;
            _workplacFirstToEndParent = workplacFirstToEndParent;
            _getWorkPlace = getWorkPlace;
            _childrenWorkPlace = childrenWorkPlace;
            }

        // نمایش فرم ورود/خروج
        public IActionResult AttendanceRecord()
            {
            ViewBag.currentTime = DateTime.Now.ToPersianDateTimeStrFarsi();
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

            ViewBag.ip = ip;
            return View();
            }

        // ثبت ورود/خروج
        [HttpPost]
        public async Task<IActionResult> AttendanceRecord(AttendanceType type)
            {
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

            var log = new AttendanceLog
                {
                Ip = ip,
                EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Timestamp = (type == 0 ? DateTime.Now.AddMinutes(-10) : DateTime.Now),
                Type = type
                };

            _context.AttendanceLogs.Add(log);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"ثبت {type} با موفقیت انجام شد.";
            return RedirectToAction("MyLogsAttendance");
            }

        public async Task<IActionResult> MyLogsAttendance(int? page)
            {
            int pageSize = 15;
            int pageNumber = page ?? 1;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var logs = await _context.AttendanceLogs
                .Where(l => l.EmployeeId == userId)
                .OrderBy(l => l.Timestamp)
                .ToListAsync();

            var grouped = logs
                .GroupBy(l => l.Timestamp.Date)
                .Select(g => new AttendanceDailyListViewModel
                    {
                    Date = g.Key,
                    Records = g.OrderBy(x => x.Timestamp)
                        .Select(x => new AttendanceRecordItem
                            {
                            Time = x.Timestamp,
                            Type = x.Type
                            })
                        .ToList()
                    })
                .OrderByDescending(x => x.Date)
                .ToPagedList(pageNumber, pageSize);

            return View(grouped);
            }


        // نمایش فرم درخواست
        public IActionResult Create()
            {
            return View();
            }

        // ارسال درخواست
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequest model)
            {
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.Status = LeaveStatus.Pending;
            model.RequestDate = DateTime.Now;

            // اعتبارسنجی ساده (بر اساس نوع مرخصی)
            if (model.Type == LeaveType.Hourly && (!model.FromHour.HasValue || !model.ToHour.HasValue))
                ModelState.AddModelError("", "ساعات مرخصی ساعتی را کامل وارد کنید");

            if (model.Type == LeaveType.Daily && (!model.FromDate.HasValue || !model.ToDate.HasValue))
                ModelState.AddModelError("", "تاریخ‌های مرخصی روزانه را کامل وارد کنید");


            if (model.RequestDate == null)
                {
                model.RequestDate = DateTime.Now;
                }
            if (!ModelState.IsValid)
                return View(model);
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
            model.Ip = ip;
            _context.LeaveRequests.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyRequests");
            }


        public async Task<IActionResult> MyRequests(int page = 1, int pageSize = 10)
            {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.LeaveRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegesterAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(items);
            }
        [Authorize(Roles = "Administrator ,TrafficManager")] // فقط مدیر مجاز به مشاهده
        public async Task<IActionResult> AllRequests(int? page)
            {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var requests = _context.LeaveRequests
                .Include(r => r.Employee)
                .OrderByDescending(r => r.RequestDate);

            var pagedRequests = await requests.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedRequests);
            }

        // تأیید یا رد کردن توسط مدیر
        [Authorize(Roles = "Administrator ,TrafficManager")] // فقط مدیر مجاز به مشاهده
        public async Task<IActionResult> Approve(int id)
            {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request == null)
                {
                return NotFound();
                }

            // تأیید اطلاعات
            request.Status = LeaveStatus.Approved;
            request.ConfirmUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.ConfirmDateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Message"] = "درخواست با موفقیت تأیید شد.";
            return RedirectToAction("AllRequests");
            }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Reject(int id)
            {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request == null)
                {
                return NotFound();
                }

            request.Status = LeaveStatus.Rejected;
            request.ConfirmUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.ConfirmDateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Message"] = "درخواست رد شد.";
            return RedirectToAction("AllRequests");
            }


        public async Task<IActionResult> AttendanceSummary(int? page, string q = "", string mcode = "", string search = "" ,long worckpalce=0)
            {
            if (search == "clear")
                {
                return RedirectToAction("AttendanceSummary");
                }

            int pageSize = 10;
            int pageNumber = page ?? 1;
            var traficUserAccess = _context.TraficeUserAccesses.AsNoTracking().Where(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            ViewData["WorkPlaces"] = _getWorkPlace.Execute(null).Data;
            if (User.IsInRole("Administrator") || User.IsInRole("TrafficManager") || traficUserAccess != null)
                {


                var employeesWithLogs = _context.AttendanceLogs
                                     .Where(a => a.Type == AttendanceType.Entry || a.Type == AttendanceType.Exit).OrderByDescending(a => a.Timestamp)
                                     .Select(a => a.Employee)
                                     .Distinct()
                                     .Select(p => new UserShowPersonDto
                                         {
                                         FirstName = p.FirstName,
                                         LastName = p.LastName,
                                         WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent((long)p.WorkPlaceId).Data,
                                         Phone = p.Phone,
                                         UserName = p.UserName,
                                         Id = p.Id
                                         });
                if (!User.IsInRole("Administrator"))
                {
                    if (traficUserAccess != null)
                        {
                        var worckplaces = _childrenWorkPlace.ExequteById(traficUserAccess.WorkPlaceId).Data;
                        var UserIdes = _context.Users.AsNoTracking().Where(p => worckplaces.Contains((long)p.WorkPlaceId)).Select(p => p.Id).ToList();
                        employeesWithLogs = employeesWithLogs.Where(p => UserIdes.Contains(p.Id)).AsQueryable();
                        }
                    }
             
                if (worckpalce > 0)
                    {
                   
                    var UserIdes = _context.Users.AsNoTracking().Where(p =>p.WorkPlaceId== worckpalce).Select(p => p.Id).ToList();
                    employeesWithLogs = employeesWithLogs.Where(p => UserIdes.Contains(p.Id)).AsQueryable();
                    }
                if (!String.IsNullOrEmpty(q) || !String.IsNullOrEmpty(mcode))
                    {
                    var users = _context.Users.AsQueryable();
                    if (!String.IsNullOrEmpty(q))
                        {
                        users = users.Where(p => p.LastName.Contains(q) || p.UserName.Contains(q)).AsQueryable();
                        }
                    if (!String.IsNullOrEmpty(mcode))
                        {
                        users = users.Where(p => p.melli.Contains(mcode)).AsQueryable();
                        }
                    if (users.Count() > 0)
                        {
                        employeesWithLogs = employeesWithLogs.Where(p => users.Select(o => o.Id).ToList().Contains(p.Id)).AsQueryable();
                        }
                    }

                var pagedEmployees = await employeesWithLogs.ToPagedListAsync(pageNumber, pageSize);


                return View(pagedEmployees);
                }
            return View(null);
            }
      

        //کارکنانی که ورود و خروج داشته اند
        [Authorize(Roles = "Administrator ,TrafficManager")] // فقط مدیر مجاز به مشاهده

        public async Task<IActionResult> AttendanceSummaryDaily(int page = 1)
            {

            int pageSize = 10;
            var model = await GetPagedGroupedLogsAsync(page, pageSize, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(model);
            }

        //جزئیات ورود و خروج کارکنان
        [Authorize(Roles = "Administrator ,TrafficManager")] // فقط مدیر مجاز به مشاهده


        public async Task<IActionResult> AttendanceDetailsByDate(string id, int? page)
            {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var logs = await _context.AttendanceLogs
                .Where(l => l.EmployeeId == id)
                .OrderBy(l => l.Timestamp)
                .ToListAsync();

            var groupedLogs = logs
                .GroupBy(l => l.Timestamp.Date)
                .Select(g => new DailyAttendanceListViewModel
                    {
                    Date = g.Key,
                    Records = g.OrderBy(x => x.Timestamp)
                        .Select(x => new AttendanceRecordItem
                            {
                            Time = x.Timestamp,
                            Type = x.Type,
                            Ip=x.Ip
                            })
                        .ToList()
                    })
                .OrderByDescending(x => x.Date)
                .ToPagedList(pageNumber, pageSize);
            var user = _context.Users.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
            ViewBag.EmployeeName = user.FirstName + "  " + user.LastName;

            ViewBag.EmployeeId = id;
            return View(groupedLogs);
            }



        // -------------------------------
        // اکشن GET: نمایش فرم اعطای دسترسی
        // -------------------------------
        // GET: User/GrantAccess/{userId}
        [Authorize(Roles = "Administrator")] // فقط مدیر مجاز به مشاهده
        public async Task<IActionResult> GrantAccess(string userId)
            {
            var user = _context.Users.Where(p => p.Id == userId).FirstOrDefault();
            if (user == null) return NotFound();

            // بارگذاری دسترسی‌های فعلی
            var existing = await _context.TraficeUserAccesses
                .Where(a => a.UserId == userId)
                .Include(a => a.WorkPlace)
                .ToListAsync();

            var vm = new GrantAccessViewModel
                {
                UserId = userId,
                UserName = $"{user.FirstName} {user.LastName}",
                ExistingAccesses = existing
                    .Select(a => new UserAccessItem
                        {
                        AccessId = a.Id,
                        WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent((long)a.WorkPlaceId).Data,
                        Level = (int)a.Level
                        })
                    .ToList(),
                WorkPlaces = _getWorkPlace.Execute(null).Data,
                SelectedWorkPlaceId = null,
                SelectedAccessLevel = Models.ApprovalLevel.City, // مقدار پیش‌فرض
                AccessLevels = Enum.GetValues(typeof(Models.ApprovalLevel))
                    .Cast<Models.ApprovalLevel>()
                    .Select(l => new SelectListItem
                        {
                        Value = ((int)l).ToString(),
                        Text = l.ToString()
                        })
                    .ToList()
                };
            return View(vm);
            }

        // POST: /User/GrantAccess
        [HttpPost]
        [Authorize(Roles = "Administrator")] // فقط مدیر مجاز به مشاهده
        [ValidateAntiForgeryToken]
        public IActionResult GrantAccess(GrantAccessViewModel model)
            {
            if (!ModelState.IsValid)
                {
                var user = _context.Users.Where(p => p.Id == model.UserId).FirstOrDefault();
                if (user == null) return NotFound();

                // بارگذاری دسترسی‌های فعلی
                var existing = _context.TraficeUserAccesses
                    .Where(a => a.UserId == model.UserId)
                    .Include(a => a.WorkPlace)
                    .ToList();

                var vm = new GrantAccessViewModel
                    {
                    UserId = model.UserId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    ExistingAccesses = existing
                        .Select(a => new UserAccessItem
                            {
                            AccessId = a.Id,
                            WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent((long)a.WorkPlaceId).Data,
                            Level = (int)a.Level
                            })
                        .ToList(),
                    WorkPlaces = _getWorkPlace.Execute(null).Data,
                    SelectedWorkPlaceId = null,
                    SelectedAccessLevel = Models.ApprovalLevel.City, // مقدار پیش‌فرض
                    AccessLevels = Enum.GetValues(typeof(Models.ApprovalLevel))
                        .Cast<Models.ApprovalLevel>()
                        .Select(l => new SelectListItem
                            {
                            Value = ((int)l).ToString(),
                            Text = l.ToString()
                            })
                        .ToList()
                    };

                return View(vm);
                }



            if (_context.TraficeUserAccesses.Where(p => p.UserId == model.UserId).Any())
                {
                ModelState.AddModelError("AccessDoublicat", "برای کاربر در قبل دسترسی ایجاد گردیده است!!!!");
                var user = _context.Users.Where(p => p.Id == model.UserId).FirstOrDefault();
                if (user == null) return NotFound();

                // بارگذاری دسترسی‌های فعلی
                var existing = _context.TraficeUserAccesses
                    .Where(a => a.UserId == model.UserId)
                    .Include(a => a.WorkPlace)
                    .ToList();

                var vm = new GrantAccessViewModel
                    {
                    UserId = model.UserId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    ExistingAccesses = existing
                        .Select(a => new UserAccessItem
                            {
                            AccessId = a.Id,
                            WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent((long)a.WorkPlaceId).Data,
                            Level = (int)a.Level
                            })
                        .ToList(),
                    WorkPlaces = _getWorkPlace.Execute(null).Data,
                    SelectedWorkPlaceId = null,
                    SelectedAccessLevel = Models.ApprovalLevel.City, // مقدار پیش‌فرض
                    AccessLevels = Enum.GetValues(typeof(Models.ApprovalLevel))
                        .Cast<Models.ApprovalLevel>()
                        .Select(l => new SelectListItem
                            {
                            Value = ((int)l).ToString(),
                            Text = l.ToString()
                            })
                        .ToList()
                    };

                return View(vm);
                }
            _context.TraficeUserAccesses.Add(new TraficeUserAccess
                {
                UserId = model.UserId,
                WorkPlaceId = (long)model.SelectedWorkPlaceId,
                Level = (Azmoon.Domain.Entities.ApprovalLevel)model.SelectedAccessLevel
                });
            _context.SaveChanges();
            return RedirectToAction("AttendanceSummary", "HourlyLeave");
            }

        // POST: User/DeleteAccess
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccess(long accessId, string userId)
            {
            var item = await _context.TraficeUserAccesses.FindAsync(accessId);
            if (item != null)
                {
                _context.TraficeUserAccesses.Remove(item);
                await _context.SaveChangesAsync();
                }
            return RedirectToAction(nameof(GrantAccess), new { userId });
            }

        public async Task<IPagedList<AttendanceGroupKey>> GetPagedGroupKeysAsync(int page, int pageSize)
            {
            // 1. گروه‌بندی مستقیم در دیتابیس بر اساس EmployeeId و تاریخ
            // 2. برای هر گروه، بزرگ‌ترین Timestamp (یا هر معیار ترتیب)
            // 3. مرتب‌سازی نزولی روی این معیار
            // 4. اعمال Skip/Take برای صفحه‌بندی
            var keys = await _context.AttendanceLogs
                .GroupBy(a => new { a.EmployeeId, Date = a.Timestamp.Date })
                .Select(g => new AttendanceGroupKey
                    {
                    EmployeeId = g.Key.EmployeeId,
                    Date = g.Key.Date,
                    LatestTimestamp = g.Max(x => x.Timestamp)
                    })
                .OrderByDescending(k => k.LatestTimestamp)
                .ToPagedListAsync(page, pageSize);

            return keys;
            }



        public async Task<IPagedList<AttendanceLogGroupViewModel>>
         GetPagedGroupedLogsAsync(int page, int pageSize, string userId)
            {
            // الف) کوئری گروه‌بندی و مرتب‌سازی کلیدها

            var traficUserAccess = _context.TraficeUserAccesses.AsNoTracking().Where(p => p.UserId == userId).FirstOrDefault();
            if (User.IsInRole("Administrator"))
                {
                var groupKeysQuery = _context.AttendanceLogs
                      .GroupBy(a => new { a.EmployeeId, Date = a.Timestamp.Date })
                      .Select(g => new AttendanceGroupKey
                          {
                          EmployeeId = g.Key.EmployeeId,
                          Date = g.Key.Date,
                          LatestTimestamp = g.Max(x => x.Timestamp)
                          })
                      .OrderByDescending(x => x.LatestTimestamp);

                // شمارش کل گروه‌ها (برای صفحه‌بندی)
                var totalCount = await groupKeysQuery.CountAsync();

                // واکشی کلیدهای صفحه فعلی
                var pageKeys = await groupKeysQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // استخراج لیست‌های EmployeeId و Date برای کوئری بعدی
                var employeeIds = pageKeys.Select(k => k.EmployeeId).Distinct().ToList();
                var dates = pageKeys.Select(k => k.Date).Distinct().ToList();

                // ب) واکشی رکوردهای مرتبط با آن صفحه کلیدها
                var logs = await _context.AttendanceLogs
                    .Include(a => a.Employee)
                    .Where(a => employeeIds.Contains(a.EmployeeId)
                             && dates.Contains(a.Timestamp.Date))
                    .ToListAsync();

                // ج) نگاشت به ViewModel در حافظه
                var groups = pageKeys.Select(key =>
                {
                    var fullName = logs
                        .First(r => r.EmployeeId == key.EmployeeId
                                  && r.Timestamp.Date == key.Date)
                        .Employee;

                    var activities = logs
                        .Where(r => r.EmployeeId == key.EmployeeId
                                 && r.Timestamp.Date == key.Date)
                        .OrderBy(r => r.Timestamp)
                        .Select(r => new AttendanceLogItem
                            {
                            Time = r.Timestamp,
                            Ip = r.Ip,
                            Type = r.Type == AttendanceType.Entry ? "ورود" : "خروج"
                            })
                        .ToList();

                    return new AttendanceLogGroupViewModel
                        {
                        Date = key.Date,
                        WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent((long)fullName.WorkPlaceId).Data,
                        FullName = fullName.FirstName + " " + fullName.LastName,
                        Activities = activities
                        };
                }).ToList();

                // د) ساخت IPagedList نهایی با همان اطلاعات صفحه‌بندی
                return new StaticPagedList<AttendanceLogGroupViewModel>(
                    groups, page, pageSize, totalCount);
                }
            if (traficUserAccess != null )
                {
                var worckplaces = _childrenWorkPlace.ExequteById(traficUserAccess.WorkPlaceId).Data;
                var UserIdes = _context.Users.AsNoTracking().Where(p => worckplaces.Contains((long)p.WorkPlaceId)).Select(p => p.Id).ToList();

                var groupKeysQuery = _context.AttendanceLogs.Where(p => UserIdes.Contains(p.EmployeeId))
                                     .GroupBy(a => new { a.EmployeeId, Date = a.Timestamp.Date })
                                     .Select(g => new AttendanceGroupKey
                                         {
                                         EmployeeId = g.Key.EmployeeId,
                                         Date = g.Key.Date,
                                         LatestTimestamp = g.Max(x => x.Timestamp)
                                         })
                                     .OrderByDescending(x => x.LatestTimestamp);
                   
                // شمارش کل گروه‌ها (برای صفحه‌بندی)
                var totalCount = await groupKeysQuery.CountAsync();

                // واکشی کلیدهای صفحه فعلی
                var pageKeys = await groupKeysQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // استخراج لیست‌های EmployeeId و Date برای کوئری بعدی
                var employeeIds = pageKeys.Select(k => k.EmployeeId).Distinct().ToList();
                var dates = pageKeys.Select(k => k.Date).Distinct().ToList();

                // ب) واکشی رکوردهای مرتبط با آن صفحه کلیدها
                var logs = await _context.AttendanceLogs
                    .Include(a => a.Employee)
                    .Where(a => employeeIds.Contains(a.EmployeeId)
                             && dates.Contains(a.Timestamp.Date))
                    .ToListAsync();

                // ج) نگاشت به ViewModel در حافظه
                var groups = pageKeys.Select(key =>
                {
                    var fullName = logs
                        .First(r => r.EmployeeId == key.EmployeeId
                                  && r.Timestamp.Date == key.Date)
                        .Employee;

                    var activities = logs
                        .Where(r => r.EmployeeId == key.EmployeeId
                                 && r.Timestamp.Date == key.Date)
                        .OrderBy(r => r.Timestamp)
                        .Select(r => new AttendanceLogItem
                            {
                            Time = r.Timestamp,
                            Ip=r.Ip,
                            Type = r.Type == AttendanceType.Entry ? "ورود" : "خروج"
                            })
                        .ToList();

                    return new AttendanceLogGroupViewModel
                        {
                        Date = key.Date,
                        WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent((long)fullName.WorkPlaceId).Data,
                        FullName = fullName.FirstName + " " + fullName.LastName,
                        Activities = activities
                        };
                }).ToList();

                // د) ساخت IPagedList نهایی با همان اطلاعات صفحه‌بندی
                return new StaticPagedList<AttendanceLogGroupViewModel>(
                    groups, page, pageSize, totalCount);


                }
            return null;
            }


        }

    // کمکی برای نگهداری کلید
    public class AttendanceGroupKey
        {
        public string EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestTimestamp { get; set; }
        }
    }
