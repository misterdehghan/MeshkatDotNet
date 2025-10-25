using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities
{
    public class AttendanceLog
        {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string CounfirmUserId { get; set; }
        public DateTime? CounfirmDateTime{ get; set; }
        public byte Status { get; set; } = 1;
        public string Ip { get; set; }
        public DateTime Timestamp { get; set; }   // زمان ثبت‌شده توسط سرور
        public AttendanceType Type { get; set; }  // ورود یا خروج

        public virtual User Employee { get; set; }
        }

    public enum AttendanceType
        {
        Entry,
        Exit
        }

    }
