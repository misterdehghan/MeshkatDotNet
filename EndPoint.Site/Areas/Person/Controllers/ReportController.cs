using Microsoft.AspNetCore.Mvc;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Domain.Entities;
using EndPoint.Site.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using PersianTools.Core;
using System;
using Azmoon.Application.Service.WorkTime;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;              // کلاس PersianDateTime



namespace EndPoint.Site.Areas.Person.Controllers
    {


    [Area("Person")]
    [Authorize]
    [Authorize(Roles = "Administrator ,TrafficManager")]
    public class ReportController : Controller
        {
        private readonly IDataBaseContext _context;
        private readonly IWorkTimeService _workTimeService;
        private readonly PersianCalendar _pc = new PersianCalendar();

        public ReportController(IDataBaseContext context, IWorkTimeService workTimeService)
            {
            _context = context;
            _workTimeService = workTimeService;
            }

        [HttpGet]
        public IActionResult MonthlyReportForm(string employeeId)
            {
            var user = _context.Users.Find(employeeId);
            var vm = new MonthlyReportViewModel
                {
                EmployeeId = employeeId,
                FullName = user.FirstName + " " + user.LastName
                };
            return View(vm);
            }



        // Controller: ReportController.cs

        [HttpPost]

        public async Task<IActionResult> MonthlyReportForm(MonthlyReportViewModel vm)
            {
            // ۱. تبدیل ماه و سال شمسی به بازه‌ی میلادی (از ابتدای روز اول تا انتهای آخرین روز)
            var pc = new System.Globalization.PersianCalendar();
            int daysInMonth = pc.GetDaysInMonth(vm.Year, vm.Month);
            var fromDate = pc.ToDateTime(vm.Year, vm.Month, 1, 0, 0, 0, 0);
            var toDate = pc.ToDateTime(vm.Year, vm.Month, daysInMonth, 23, 59, 59, 999);

            // ۲. بارگذاری دوره‌ی زمانی (SchedulePeriod) شامل ساعات هفتگی
            var period = await _context.SchedulePeriods
                .Include(s => s.WeeklyWorkTimes)
                .FirstOrDefaultAsync(s =>
                    s.StartDate.Date <= fromDate.Date &&
                    s.EndDate.Date >= toDate.Date);

            if (period == null)
                {
                ModelState.AddModelError("", "هیچ دوره‌ی زمانی فعالی برای این ماه تعریف نشده است.");
                return View(vm);
                }

            // ۳. واکشی مرخصی‌های روزانه و ساعتی کارمند در بازه
            var leaves = await _context.LeaveRequests
                .Where(l =>
                    l.UserId == vm.EmployeeId &&
                    l.Status == LeaveStatus.Approved &&
                    (
                        // مرخصی روزانه
                        (l.Type == LeaveType.Daily &&
                         l.FromDate.Value.Date <= toDate.Date &&
                         l.ToDate.Value.Date >= fromDate.Date)
                        ||
                        // مرخصی ساعتی
                        (l.Type == LeaveType.Hourly &&
                         l.RequestDate.Value.Date >= fromDate.Date &&
                         l.RequestDate.Value.Date <= toDate.Date)
                    ))
                .ToListAsync();

            // ۴. واکشی لاگ‌های ورود/خروج کارمند در بازه
            var logs = await _context.AttendanceLogs
                .Where(a =>
                    a.EmployeeId == vm.EmployeeId &&
                    a.Timestamp.Date >= fromDate.Date &&
                    a.Timestamp.Date <= toDate.Date)
                .OrderBy(a => a.Timestamp)
                .ToListAsync();

            // ۵. استخراج تعطیلات رسمی با PersianDateTime
            var holidays = new Dictionary<DateTime, string>();
            for (int d = 1; d <= daysInMonth; d++)
                {
                var pDate = new PersianDateTime(vm.Year, vm.Month, d);
                var infos = pDate.GetDateInformation();
                var hd = infos.FirstOrDefault(i => i.IsHoliDay && (int)i.DateType == 3);
                if (hd != null)
                    {
                    var gd = pDate.ToDateTime(CultureInfo.InvariantCulture);
                    holidays[gd.Date] = hd.Description;
                    }
                }

            // ۶. ساخت لیست روزانه با محاسبات کارکرد و اضافه‌کاری
            var days = new List<DailyReportEntry>(daysInMonth);
            for (int d = 1; d <= daysInMonth; d++)
                {
                // الف) تاریخ میلادی آن روز
                var current = pc.ToDateTime(vm.Year, vm.Month, d, 0, 0, 0, 0);

                // ب) شیفت تعریف‌شده برای روز هفته جاری
                var shift = period.WeeklyWorkTimes
                    .FirstOrDefault(w => w.DayOfWeek == current.DayOfWeek);

                // ج) لاگ‌های آن روز
                var dayLogs = logs.Where(a => a.Timestamp.Date == current.Date).ToList();

                // د) اولین ورود و آخرین خروج
                var firstEntry = dayLogs
                    .FirstOrDefault(a => a.Type == AttendanceType.Entry)?
                    .Timestamp;
                var lastExit = dayLogs
                    .LastOrDefault(a => a.Type == AttendanceType.Exit)?
                    .Timestamp;

                // هـ) تشخیص تردد ناقص (ورود یا خروج موجود باشد اما دیگری نباشد)
                bool isIncomplete = firstEntry.HasValue ^ lastExit.HasValue;

                // و) محاسبه ساعت کارکرد واقعی
                TimeSpan workHours = TimeSpan.Zero;
                if (!isIncomplete && firstEntry.HasValue && lastExit.HasValue)
                    {
                    workHours = lastExit.Value - firstEntry.Value;
                    }

                // ز) محاسبه اضافه‌کاری
                TimeSpan overtime = TimeSpan.Zero;

                // 1. اگر تعطیل رسمی یا جمعه است → کل workHours اضافه‌کاری محسوب شود
                if ((holidays.ContainsKey(current.Date) ||
                     current.DayOfWeek == DayOfWeek.Friday)
                    && workHours > TimeSpan.Zero)
                    {
                    overtime = workHours;
                    workHours = TimeSpan.Zero; // (اختیاری) مخفی کردن کارکرد روز تعطیل
                    }
                // 2. در روزهای عادی با شیفت تعریف‌شده
                else if (shift != null && lastExit.HasValue)
                    {
                    var scheduledEnd = shift.EndTime;
                    var actualExit = lastExit.Value.TimeOfDay;
                    if (actualExit > scheduledEnd)
                        {
                        overtime = actualExit - scheduledEnd;
                        }
                    }

                // ح) جمع‌آوری اطلاعات روزانه
                days.Add(new DailyReportEntry
                    {
                    Date = current,
                    Occurrence = holidays.TryGetValue(current.Date, out var occ) ? occ : null,
                    HasDailyLeave = leaves.Any(l =>
                        l.Type == LeaveType.Daily &&
                        l.FromDate.Value.Date <= current.Date &&
                        l.ToDate.Value.Date >= current.Date),
                    HourlyLeaves = leaves
                        .Where(l => l.Type == LeaveType.Hourly &&
                                    l.RequestDate.Value.Date == current.Date)
                        .Select(l => (
                            From: l.FromHour.Value.TimeOfDay,
                            To: l.ToHour.Value.TimeOfDay))
                        .OrderBy(t => t.From)
                        .ToList(),
                    AttendanceCount = dayLogs.Count,
                    FirstEntry = firstEntry,
                    LastExit = lastExit,
                    IsIncomplete = isIncomplete,
                    WorkHours = workHours,
                    Overtime = overtime
                    });
                }

            // ۷. پر کردن نام کارمند و ارسال داده‌ها به ویو
            vm.FullName = await _context.Users
                .Where(u => u.Id == vm.EmployeeId)
                .Select(u => u.FirstName + " " + u.LastName)
                .FirstOrDefaultAsync();

            vm.Days = days;
            return View("MonthlyReportResult", vm);
            }






        }
    }


