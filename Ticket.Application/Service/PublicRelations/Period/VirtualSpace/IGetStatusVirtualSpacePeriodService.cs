using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.VirtualSpace
{
    public interface IGetStatusVirtualSpacePeriodService
    {
        ResultDto Execute(int ChannelId);
    }

    public class GetStatisticalStatus : IGetStatusVirtualSpacePeriodService
    {
        private readonly IDataBaseContext _context;

        public GetStatisticalStatus(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int ChannelId)
        {
            // یافتن اولین دوره آماری فعال
            var activePeriod = _context.VirtualSpacePeriod.Where(p => p.IsRemoved == false).FirstOrDefault(stat => DateTime.Now >= stat.StartDate && DateTime.Now <= stat.EndDate);

            if (activePeriod != null)
            {
                var memberStatistics = _context.MembersPeriods
                    .Where(p => p.ChannelId == ChannelId)
                    .Where(p => p.VirtualSpacePeriodId == activePeriod.Id)
                    .FirstOrDefault(); // Change this to FirstOrDefault() to get a single result or null.

                if (memberStatistics != null)
                {
                    var result = new ResultDto()
                    {
                        IsSuccess = true,
                        Message = "آمار" + " " + activePeriod.StatisticalPeriod + " " + "با موفقیت ثبت گردید",
                    };
                    return result;
                }
                else
                {
                    var result = new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "لطفا آمار" + " " + activePeriod.StatisticalPeriod + " " + "را وارد کنید",
                    };
                    return result;
                }
            }
            else
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "دوره آماری فعال وجود ندارد",
                };
            }
        }
    }
}

