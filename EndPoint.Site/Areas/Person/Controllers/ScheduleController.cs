using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Domain.Entities;
using EndPoint.Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Person.Controllers
{
    [Area("Person")]
    [Authorize]
    [Authorize(Roles = "Administrator ,TrafficManager")]
    public class ScheduleController : Controller
        {
        private readonly IDataBaseContext _context;

        public ScheduleController(IDataBaseContext context)
            {
            _context = context;
            }
        // Index: فهرست دوره‌ها
        public async Task<IActionResult> Index()
            {
            var list = await _context.SchedulePeriods
                .OrderByDescending(s => s.Id)
                .ToListAsync();
            return View(list);
            }

        // Create: قبلاً پیاده شد

        // Edit GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
            {
            var period = await _context.SchedulePeriods
                .Include(s => s.WeeklyWorkTimes)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (period == null) return NotFound();

            var vm = new SchedulePeriodEditViewModel(period);
            return View(vm);
            }
        [HttpGet]
        public IActionResult CreatePeriod()
            {
            var vm = new SchedulePeriodCreateViewModel();
            return View(vm);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePeriod(SchedulePeriodCreateViewModel vm)
            {
            if (!ModelState.IsValid)
                {
                return View(vm);
                }

            var period = new SchedulePeriod
                {
                StartDate = vm.FromDate.Value,
                EndDate = vm.ToDate.Value,
                WeeklyWorkTimes = vm.Days.Select(d => new WeeklyWorkTime
                    {
                    DayOfWeek = d.DayOfWeek,
                    StartTime = TimeSpan.ParseExact(d.StartTime, "hh\\:mm", CultureInfo.InvariantCulture),
                    EndTime = TimeSpan.ParseExact(d.EndTime, "hh\\:mm", CultureInfo.InvariantCulture)
                    }).ToList()
                };

            _context.SchedulePeriods.Add(period);
            await _context.SaveChangesAsync();

            // Redirect یا نمایش پیغام موفقیت
            return RedirectToAction("Index", "Schedule");
            }

  

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SchedulePeriodEditViewModel vm)
            {
            if (!ModelState.IsValid) return View(vm);

            var period = await _context.SchedulePeriods
                .Include(s => s.WeeklyWorkTimes)
                .FirstOrDefaultAsync(s => s.Id == vm.Id);
            if (period == null) return NotFound();

            // بروزرسانی بازه
            period.StartDate = vm.FromDate.Value;
            period.EndDate = vm.ToDate.Value;

            // حذف ساعات قبلی و ثبت جدید
            _context.WeeklyWorkTimes.RemoveRange(period.WeeklyWorkTimes);
            period.WeeklyWorkTimes = vm.Days.Select(d => new WeeklyWorkTime
                {
                SchedulePeriodId = period.Id,
                DayOfWeek = d.DayOfWeek,
                StartTime = TimeSpan.ParseExact(d.StartTime, "hh\\:mm", CultureInfo.InvariantCulture),
                EndTime = TimeSpan.ParseExact(d.EndTime, "hh\\:mm", CultureInfo.InvariantCulture)
                }).ToList();

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            }

        // Delete POST
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
            {
            var period = await _context.SchedulePeriods.FindAsync(id);
            if (period != null)
                {
                _context.SchedulePeriods.Remove(period);
                await _context.SaveChangesAsync();
                }
            return RedirectToAction("Index");
            }
        }
    }
