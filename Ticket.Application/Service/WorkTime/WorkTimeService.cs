using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.WorkTime
{
    public interface IWorkTimeService
        {
        Task<List<WorkTimeResult>> GetWorkTimeReportAsync(string employeeId, int schedulePeriodId);

        }
    public class WorkTimeService : IWorkTimeService
        {
        private readonly IDataBaseContext _context;
        private readonly PersianCalendar _pc = new PersianCalendar();

        public WorkTimeService(IDataBaseContext context)
            {
            _context = context;
            }

        public async Task<List<WorkTimeResult>> GetWorkTimeReportAsync(
            string employeeId, int schedulePeriodId)
            {
            // ۱. بارگذاری دوره به همراه ساعات هفتگی
            var period = await _context.SchedulePeriods
                .Include(s => s.WeeklyWorkTimes)
                .FirstOrDefaultAsync(s => s.Id == schedulePeriodId);
            if (period == null) return new List<WorkTimeResult>();

            // ۲. واکشی لاگ‌های تردد در بازه
            var logs = await _context.AttendanceLogs
                .Where(a =>
                    a.EmployeeId == employeeId &&
                    a.Timestamp.Date >= period.StartDate.Date &&
                    a.Timestamp.Date <= period.EndDate.Date)
                .OrderBy(a => a.Timestamp)
                .ToListAsync();

            var results = new List<WorkTimeResult>();
            for (var date = period.StartDate.Date; date <= period.EndDate.Date; date = date.AddDays(1))
                {
                // ۳. پیدا کردن الگو برای آن روز هفته
                var dailyPattern = period.WeeklyWorkTimes
                    .FirstOrDefault(w => w.DayOfWeek == date.DayOfWeek);
                if (dailyPattern == null) continue;

                // ۴. محاسبه اولین ورود و آخرین خروج
                var dayLogs = logs.Where(l => l.Timestamp.Date == date).ToList();
                var firstEntry = dayLogs.FirstOrDefault(l => l.Type == AttendanceType.Entry)?.Timestamp;
                var lastExit = dayLogs.LastOrDefault(l => l.Type == AttendanceType.Exit)?.Timestamp;

                // ۵. محاسبه ساعت مفید
                var effective = TimeSpan.Zero;
                if (firstEntry.HasValue && lastExit.HasValue)
                    {
                    var actualStart = firstEntry.Value.TimeOfDay < dailyPattern.StartTime
                        ? dailyPattern.StartTime : firstEntry.Value.TimeOfDay;
                    var actualEnd = lastExit.Value.TimeOfDay > dailyPattern.EndTime
                        ? dailyPattern.EndTime : lastExit.Value.TimeOfDay;
                    if (actualEnd > actualStart)
                        effective = actualEnd - actualStart;
                    }

                results.Add(new WorkTimeResult
                    {
                    Date = date,
                    DayName = CultureInfo.GetCultureInfo("fa-IR")
                                                .DateTimeFormat.GetDayName(date.DayOfWeek),
                    ScheduledStart = dailyPattern.StartTime,
                    ScheduledEnd = dailyPattern.EndTime,
                    ActualFirstEntry = firstEntry,
                    ActualLastExit = lastExit,
                    EffectiveWork = effective
                    });
                }

            return results;
            }
        }
    // نتیجه‌ی گزارش برای هر روز
    public class WorkTimeResult
        {
        public DateTime Date { get; set; }
        public string DayName { get; set; }
        public TimeSpan ScheduledStart { get; set; }
        public TimeSpan ScheduledEnd { get; set; }
        public DateTime? ActualFirstEntry { get; set; }
        public DateTime? ActualLastExit { get; set; }
        public TimeSpan EffectiveWork { get; set; }
        }
    // تعریف زمان‌های ورودی/خروج برای یک روز هفته
    public class DailyWorkTime
        {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public DailyWorkTime(DayOfWeek day, TimeSpan start, TimeSpan end)
            {
            DayOfWeek = day;
            StartTime = start;
            EndTime = end;
            }


        }
    public class SchedulePeriod
        {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DailyWorkTime> DailyWorkTimes { get; set; }
        }
    }
