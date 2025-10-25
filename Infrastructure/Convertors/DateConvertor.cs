using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }

        public static int ConvertYearToShamsi(int gregorianYear)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(gregorianYear, 3, 21); // شروع سال شمسی در 1 فروردین
            return pc.GetYear(dt);
        }


        // متدی برای تبدیل تاریخ شمسی به میلادی
        public static DateTime ToGregorian(string persianDate)
        {
            PersianCalendar pc = new PersianCalendar();
            var parts = persianDate.Split('/');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);
            return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
    }
}
