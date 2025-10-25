using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities
{
    public class SchedulePeriod
        {
        public int Id { get; set; }

        // تاریخ شمسی یا میلادی بسته به نیاز شما
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // رابطه یک‌به‌چند با ساعات هفتگی
        public ICollection<WeeklyWorkTime> WeeklyWorkTimes { get; set; }
        }

    }
