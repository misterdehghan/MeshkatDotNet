using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.DashboardServices
{
    public interface IGetStatusVirtualSpacePeriodForDashboardService
    {
        ResultDto Execute();
    }

    public class GetStatusVirtualSpacePeriodForDashboardService : IGetStatusVirtualSpacePeriodForDashboardService
    {
        private readonly IDataBaseContext _context;

        public GetStatusVirtualSpacePeriodForDashboardService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute()
        {
            // یافتن اولین دوره آماری فعال
            var activePeriod = _context.VirtualSpacePeriod.Where(p => p.IsRemoved == false).FirstOrDefault(stat => DateTime.Now >= stat.StartDate && DateTime.Now <= stat.EndDate);
            if (activePeriod != null)
            {
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "جهت ثبت اطلاعات کلیک کنید",
                };
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
