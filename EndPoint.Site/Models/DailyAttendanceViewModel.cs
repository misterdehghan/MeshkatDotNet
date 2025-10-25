using Azmoon.Application.Service.WorkPlace.Dto;
using Azmoon.Application.Service.WorkTime;
using Azmoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace EndPoint.Site.Models
    {
    public class DailyAttendanceViewModel
        {
        public DateTime Date { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        }
    public class AttendanceDailyListViewModel
        {
        public DateTime Date { get; set; }
        public List<AttendanceRecordItem> Records { get; set; }
        }

    public class AttendanceRecordItem
        {
        public DateTime Time { get; set; }
        public AttendanceType Type { get; set; }
        public string Ip { get; set; }
        }
    public class DailyAttendanceListViewModel
        {
        public DateTime Date { get; set; }
        public List<AttendanceRecordItem> Records { get; set; }
        }

    public class AttendanceLogGroupViewModel
        {
        public DateTime Date { get; set; }
        public string FullName { get; set; }
        public string WorkPlaceName { get; set; }
        public List<AttendanceLogItem> Activities { get; set; }
        }

    public class AttendanceLogItem
        {
        public DateTime Time { get; set; }
        public string Type { get; set; } // ورود یا خروج

        public string Ip { get; set; } // ورود یا خروج
        }








    // ۱. افزودن فیلدهای مناسبت (تعطیلی) به ViewModel
    // ۱) به‌روزرسانی کلاس DailyReportEntry
    public class DailyReportEntry
        {
        public DateTime Date { get; set; }
        public string Occurrence { get; set; }
        public bool HasDailyLeave { get; set; }
        public List<(TimeSpan From, TimeSpan To)> HourlyLeaves { get; set; }
        public int AttendanceCount { get; set; }
        public DateTime? FirstEntry { get; set; }
        public DateTime? LastExit { get; set; }

        // دو فیلد جدید
        public bool IsIncomplete { get; set; }
        public TimeSpan WorkHours { get; set; }
        public TimeSpan Overtime { get; set; }

        public bool IsHoliday => !string.IsNullOrEmpty(this.Occurrence);
        }



    public class MonthlyReportViewModel
        {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<DailyReportEntry> Days { get; set; }
        }

    public class HourlyLeaveItem
        {
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        }
    // ViewModel برای فرم و نتایج گزارش
    public class WorkTimeFormViewModel
        {
        [Required(ErrorMessage = "انتخاب کارمند الزامی است")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "انتخاب دوره زمانی الزامی است")]
        public int SchedulePeriodId { get; set; }

        // فیلدهای کمکی برای DropDownList
        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> SchedulePeriods { get; set; }

        // پس از ارسال فرم، این بخش پر می‌شود
        public List<WorkTimeResult> Results { get; set; }

        }

    public class WeeklyWorkTimeItem
        {
        public DayOfWeek DayOfWeek { get; set; }

        public string DayName { get; set; }

        [Required(ErrorMessage = "ساعت شروع را وارد کنید")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "ساعت پایان را وارد کنید")]
        public string EndTime { get; set; }
        }

    public class SchedulePeriodCreateViewModel
        {
        [Required(ErrorMessage = "از تاریخ را انتخاب کنید")]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "تا تاریخ را انتخاب کنید")]
        public DateTime? ToDate { get; set; }

        public List<WeeklyWorkTimeItem> Days { get; set; }

        public SchedulePeriodCreateViewModel()
            {
            // همه روزهای هفته به جز جمعه
            var persian = new CultureInfo("fa-IR");
            Days = Enum.GetValues(typeof(DayOfWeek))
                       .Cast<DayOfWeek>()
                       .Where(d => d != DayOfWeek.Friday)
                       .Select(d => new WeeklyWorkTimeItem
                           {
                           DayOfWeek = d,
                           DayName = persian.DateTimeFormat.GetDayName(d)
                           })
                       .ToList();
            }
        }

    public class SchedulePeriodEditViewModel
        {
        public int Id { get; set; }

        [Required(ErrorMessage = "از تاریخ را انتخاب کنید")]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "تا تاریخ را انتخاب کنید")]
        public DateTime? ToDate { get; set; }

        public List<WeeklyWorkTimeItem> Days { get; set; }

        public SchedulePeriodEditViewModel() : this(null) { }

        public SchedulePeriodEditViewModel(Azmoon.Domain.Entities.SchedulePeriod period)
            {
            var persian = new CultureInfo("fa-IR");
            Days = Enum.GetValues(typeof(DayOfWeek))
                       .Cast<DayOfWeek>()
                       .Where(d => d != DayOfWeek.Friday)
                       .Select(d => new WeeklyWorkTimeItem
                           {
                           DayOfWeek = d,
                           DayName = persian.DateTimeFormat.GetDayName(d)
                           })
                       .ToList();

            if (period != null)
                {
                Id = period.Id;
                FromDate = period.StartDate;
                ToDate = period.EndDate;
                foreach (var w in period.WeeklyWorkTimes)
                    {
                    var itm = Days.First(x => x.DayOfWeek == w.DayOfWeek);
                    itm.StartTime = w.StartTime.ToString(@"hh\:mm");
                    itm.EndTime = w.EndTime.ToString(@"hh\:mm");
                    }
                }
            }
        }


    public class GrantAccessViewModel
        {
        public string UserId { get; set; }
        public string UserName { get; set; }

        // دسترسی‌های موجود برای نمایش و حذف
        public List<UserAccessItem> ExistingAccesses { get; set; }

        // فهرست تخت محل‌های خدمت برای اعطا
        public List<GetWorkPlaceViewModel> WorkPlaces { get; set; }
        public long? SelectedWorkPlaceId { get; set; }

        [Display(Name = "سطح تأییدکننده")]
        public ApprovalLevel SelectedAccessLevel { get; set; }
        public List<SelectListItem> AccessLevels { get; set; }
        }

    public class UserAccessItem
        {
        public long AccessId { get; set; }
        public string WorkPlaceName { get; set; }
        public int Level { get; set; }
        }
    public enum ApprovalLevel
        {
        Country = 0,
        Province = 1,
        City = 2
        }
    }

