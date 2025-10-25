using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities
{
    public class WeeklyWorkTime
        {
        public int Id { get; set; }

        // کلید خارجی به SchedulePeriod
        public int SchedulePeriodId { get; set; }
        public SchedulePeriod SchedulePeriod { get; set; }

        // روز هفته (0 = Sunday … 6 = Saturday)
        public DayOfWeek DayOfWeek { get; set; }

        // ساعت شروع و پایان (TimeSpan مناسب ذخیره‌ی ساعت)
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        }

    }
