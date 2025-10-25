using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System.Collections.Generic;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.DashboardServices
{
    public interface IReviewingMembersBasedOnTheStatisticalPeriod
    {
        ResultDto<List<MessengerPeriodResult>> Execute();
    }

    public class MessengerPeriodResult
    {
        public string Period { get; set; }
        public int TotalMembers { get; set; }
    }


    public class ReviewingMembersBasedOnTheStatisticalPeriod : IReviewingMembersBasedOnTheStatisticalPeriod
    {
        private readonly IDataBaseContext _context;

        public ReviewingMembersBasedOnTheStatisticalPeriod(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<MessengerPeriodResult>> Execute()
        {
            var period = _context.VirtualSpacePeriod;


            var result = _context.MembersPeriods
                .Where(p => p.IsRemoved == false) // فقط رکوردهایی که حذف نشده‌اند
                .GroupBy(p => p.VirtualSpacePeriodId)// گروه‌بندی بر اساس دوره آماری
                .Take(6)
            .Select(p => new MessengerPeriodResult
            {
                Period = period.FirstOrDefault(pe => pe.Id == p.Key).StatisticalPeriod, // مقدار دوره آماری
                TotalMembers = p.Sum(p => p.Member) // مجموع اعضا در این دوره
            }).ToList();

            return new ResultDto<List<MessengerPeriodResult>>
            {
                Data = result,
                IsSuccess = true,
                Message = "عملیات با موفقیت انجام شد"
            };
        }
    }
}
