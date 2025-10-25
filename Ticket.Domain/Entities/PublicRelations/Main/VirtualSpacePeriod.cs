using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.PublicRelations.Main
{
    //دوره آماری اداره فضای مجازی
    public class VirtualSpacePeriod
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsRemoved { get; set; } = false;
        public DateTime? RemoveTime { get; set; }

        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        // Property محاسبه‌شده برای بررسی وضعیت فعال بودن دوره
        public bool IsActive
        {
            get
            {
                return DateTime.Now >= StartDate && DateTime.Now <= EndDate;
            }
        }


        public ICollection<MembersPeriod> MembersPeriods { get; set; }
    }
}
