using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.DashboardServices
{
    public interface IMemberFlow
    {
        //روند افزایشی اعضا
        ResultDto<List<ResultIncreasingTrend>> IncreasingTrendOfMembers();
        //روند کاهشی اعضا
        ResultDto<List<ResultDecreasingTrend>> DecreasingTrendOfMembers();
    }

    public class ResultIncreasingTrend
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int AudienceIncrease { get; set; } // میزان افزایش مخاطب
    }



    public class ResultDecreasingTrend
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int AudienceDecrease { get; set; } // میزان کاهش مخاطب
    }

    public class MemberFlow : IMemberFlow
    {

        private readonly IDataBaseContext _context;
        public MemberFlow(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto<List<ResultIncreasingTrend>> IncreasingTrendOfMembers()
        {
            // گام 1: گرفتن دو دوره آماری آخر
            var lastTwoPeriods = _context.VirtualSpacePeriod
                .OrderByDescending(p => p.InsertTime)
                .Select(p => p.Id)
                .Take(2)  // گرفتن دو دوره آخر
                .ToList();

            if (lastTwoPeriods.Count < 2)
            {
                // بررسی کافی بودن تعداد دوره‌ها
                return new ResultDto<List<ResultIncreasingTrend>>
                {
                    IsSuccess = false,
                    Message = "Not enough periods for comparison."
                };
            }

            // جداسازی دوره آماری آخر و دوره قبلی
            var latestPeriod = lastTwoPeriods.First();
            var previousPeriod = lastTwoPeriods.Last();

            // گام 2: محاسبه تعداد اعضا برای هر کانال در دوره آخر
            var latestMembers = _context.MembersPeriods
                .Where(p => p.VirtualSpacePeriodId == latestPeriod)
                .GroupBy(p => p.ChannelId)
                .Select(g => new
                {
                    ChannelId = g.Key,  // شناسه کانال
                    LatestMembers = g.Sum(x => x.Member)  // تعداد اعضا در دوره آخر
                })
                .ToList();

            // گام 3: محاسبه تعداد اعضا برای هر کانال در دوره قبلی
            var previousMembers = _context.MembersPeriods
                .Where(p => p.VirtualSpacePeriodId == previousPeriod)
                .GroupBy(p => p.ChannelId)
                .Select(g => new
                {
                    ChannelId = g.Key,  // شناسه کانال
                    PreviousMembers = g.Sum(x => x.Member)  // تعداد اعضا در دوره قبلی
                })
                .ToList();

            // گام 4: محاسبه تعداد افزایش اعضا برای هر کانال
            var growthRates = latestMembers
                .GroupJoin(
                    previousMembers,
                    latest => latest.ChannelId,
                    previous => previous.ChannelId,
                    (latest, previous) => new
                    {
                        latest.ChannelId,
                        latest.LatestMembers,
                        PreviousMembers = previous.FirstOrDefault()?.PreviousMembers ?? 0, // مقدار صفر در صورت نبودن کانال در دوره قبلی
                        AudienceIncrease = latest.LatestMembers - (previous.FirstOrDefault()?.PreviousMembers ?? 0)
                    })
                .OrderByDescending(x => x.AudienceIncrease)  // مرتب‌سازی به ترتیب نزولی بر اساس تعداد افزایش
                .Take(5)  // گرفتن 5 کانالی که بیشترین تعداد افزایش را داشتند
                .ToList();

            // تبدیل نتیجه به فرمت مورد نیاز
            var result = growthRates.Select(gr => new ResultIncreasingTrend
            {
                ChannelId = gr.ChannelId,
                ChannelName = GetChannelName(gr.ChannelId),  // فرض بر این است که تابعی برای گرفتن نام کانال بر اساس شناسه آن وجود دارد
                AudienceIncrease = gr.AudienceIncrease,

            }).ToList();

            // بازگرداندن نتیجه نهایی
            return new ResultDto<List<ResultIncreasingTrend>>
            {
                IsSuccess = true,
                Message = "پنج کانالی که بیشترین تعداد افزایش اعضا را داشتند",
                Data = result
            };
        }

        private string GetChannelName(int channelId)
        {
            // این تابع باید نام کانال را بر اساس شناسه آن از پایگاه داده بازیابی کند
            var channel = _context.Channels.FirstOrDefault(c => c.Id == channelId);
            return channel?.ChannelName ?? "نامشخص";
        }





        public ResultDto<List<ResultDecreasingTrend>> DecreasingTrendOfMembers()
        {


            // گام 1: گرفتن دو دوره آماری آخر
            var lastTwoPeriods = _context.VirtualSpacePeriod
                .OrderByDescending(p => p.InsertTime)
                .Select(p => p.Id)
                .Take(2)
                .ToList();

            if (lastTwoPeriods.Count < 2)
            {
                return new ResultDto<List<ResultDecreasingTrend>>
                {
                    IsSuccess = false,
                    Message = "دورههای زمانی کافی برای مقایسه وجود ندارد."
                    };
            }

            // جداسازی دوره آماری آخر و دوره قبلی
            var latestPeriod = lastTwoPeriods.First();
            var previousPeriod = lastTwoPeriods.Last();

            // گام 2: محاسبه تعداد اعضا برای هر کانال در دوره آخر
            var latestMembers = _context.MembersPeriods
                .Where(p => p.VirtualSpacePeriodId == latestPeriod)
                .GroupBy(p => p.ChannelId)
                .Select(g => new
                {
                    ChannelId = g.Key,
                    LatestMembers = g.Sum(x => x.Member)
                })
                .ToList();

            // گام 3: محاسبه تعداد اعضا برای هر کانال در دوره قبلی
            var previousMembers = _context.MembersPeriods
                .Where(p => p.VirtualSpacePeriodId == previousPeriod)
                .GroupBy(p => p.ChannelId)
                .Select(g => new
                {
                    ChannelId = g.Key,
                    PreviousMembers = g.Sum(x => x.Member)
                })
                .ToList();

            // گام 4: محاسبه تعداد کاهش اعضا برای هر کانال
            var decreaseRates = latestMembers
                .GroupJoin(
                    previousMembers,
                    latest => latest.ChannelId,
                    previous => previous.ChannelId,
                    (latest, previous) => new
                    {
                        latest.ChannelId,
                        latest.LatestMembers,
                        PreviousMembers = previous.FirstOrDefault()?.PreviousMembers ?? 0,
                        AudienceDecrease = (previous.FirstOrDefault()?.PreviousMembers ?? 0) - latest.LatestMembers
                    })
                .Where(x => x.AudienceDecrease > 0) // فقط کانال‌هایی که کاهش داشته‌اند
                .OrderByDescending(x => x.AudienceDecrease) // مرتب‌سازی بر اساس بیشترین کاهش
                .Take(5) // گرفتن 5 کانال با بیشترین کاهش
                .ToList();

            // تبدیل نتیجه به فرمت مورد نیاز
            var result = decreaseRates.Select(dr => new ResultDecreasingTrend
            {
                ChannelId = dr.ChannelId,
                ChannelName = GetChannelName(dr.ChannelId),
                AudienceDecrease = dr.AudienceDecrease,
            }).ToList();

            // بازگرداندن نتیجه نهایی
            return new ResultDto<List<ResultDecreasingTrend>>
            {
                IsSuccess = true,
                Message = "پنج کانالی که بیشترین تعداد را افزایش می دهد",
                Data = result
            };

        }



    }

}
