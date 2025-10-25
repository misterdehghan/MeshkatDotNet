using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.DashboardServices
{
    public interface IGetTotalMembersFromMessengersService
    {
        ResultDto<ResultTotalMembersFromMessengers> Execute();

    }

    public class ResultTotalMembersFromMessengers
    {
        public int whatsapp { get; set; }
        public int telegram { get; set; }
        public int Instagram { get; set; }
        public int x { get; set; }
        public int sorosh { get; set; }
        public int iGap { get; set; }
        public int eeta { get; set; }
        public int bale { get; set; }
        public int rubika { get; set; }
        public int total { get; set; }
    }

    public class GetTotalMembersFromMessengersService : IGetTotalMembersFromMessengersService
    {
        private readonly IDataBaseContext _context;
        public GetTotalMembersFromMessengersService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto<ResultTotalMembersFromMessengers> Execute()
        {
            // دریافت داده‌ها و سپس انجام عملیات GroupBy و OrderByDescending در حافظه
            var whatsApp = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "واتس آپ")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var telegram = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "تلگرام")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var Instagram = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "اینستاگرام")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var x = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "ایکس")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var sorosh = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "سروش پلاس")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var iGap = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "آی گپ")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var eeta = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "ایتا")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var bale = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "بله")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            var rubika = _context.MembersPeriods
                .Where(p => p.Channel.MessengersName == "روبیکا")
                .AsEnumerable()
                .GroupBy(p => p.ChannelId)
                .Select(g => g.OrderByDescending(p => p.InsertTime).FirstOrDefault())
                .ToList();

            // محاسبه مجموع اعضا برای هر پیام‌رسان
            var totalWhatsApp = whatsApp.Sum(p => p.Member);
            var totalTelegram = telegram.Sum(p => p.Member);
            var totalInstagram = Instagram.Sum(p => p.Member);
            var totalX = x.Sum(p => p.Member);
            var totalSorosh = sorosh.Sum(p => p.Member);
            var totalIGap = iGap.Sum(p => p.Member);
            var totalEeta = eeta.Sum(p => p.Member);
            var totalBale = bale.Sum(p => p.Member);
            var totalRubika = rubika.Sum(p => p.Member);

            // محاسبه مجموع کلی
            var totalAll = totalWhatsApp + totalTelegram + totalInstagram + totalX + totalSorosh + totalIGap + totalEeta + totalBale + totalRubika;

            var resultTotalMember = new ResultTotalMembersFromMessengers
            {
                whatsapp = totalWhatsApp,
                telegram = totalTelegram,
                Instagram = totalInstagram,
                x = totalX,
                sorosh = totalSorosh,
                iGap = totalIGap,
                eeta = totalEeta,
                bale = totalBale,
                rubika = totalRubika,
                total = totalAll  // مجموع کل
            };

            return new ResultDto<ResultTotalMembersFromMessengers>
            {
                Data = resultTotalMember,
                IsSuccess = true,
                Message = "عملیات با موفقیت انجام شد"
            };
        }



    }

}
