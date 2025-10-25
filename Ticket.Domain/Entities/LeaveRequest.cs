using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities
{
    public class LeaveRequest
        {
        public int Id { get; set; }
        public string UserId { get; set; }
       

        // نوع مرخصی: ساعتی یا روزانه
        public LeaveType Type { get; set; }

        // برای مرخصی روزانه
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // برای مرخصی ساعتی
        public DateTime? FromHour { get; set; }
        public DateTime? ToHour { get; set; }
        public DateTime? RequestDate { get; set; }

        public string Reason { get; set; }

        public string ConfirmUserId { get; set; }
        public DateTime? ConfirmDateTime { get; set; }

        public LeaveStatus Status { get; set; }

        public DateTime? RegesterAt { get; set; } = DateTime.Now;

        public string Ip { get; set; }

        public virtual User Employee { get; set; }
        }

    public enum LeaveStatus
        {
        Pending,
        Approved,
        Rejected
        }

    public enum LeaveType
        {
        Hourly,
        Daily
        }

    }
