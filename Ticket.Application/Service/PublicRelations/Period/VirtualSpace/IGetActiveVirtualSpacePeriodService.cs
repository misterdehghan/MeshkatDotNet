using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.VirtualSpace
{
    public interface IGetActiveVirtualSpacePeriodService
    {
        ResultDto<ReslutActiveStatisticalDto> Execute();
    }

    public class GetActiveVirtualSpacePeriodService : IGetActiveVirtualSpacePeriodService
    {
        private readonly IDataBaseContext _context;

        public GetActiveVirtualSpacePeriodService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ReslutActiveStatisticalDto> Execute()
        {
            // یافتن اولین دوره آماری فعال
            var activePeriod = _context.VirtualSpacePeriod.Where(p => p.IsRemoved == false).FirstOrDefault(stat => DateTime.Now >= stat.StartDate && DateTime.Now <= stat.EndDate);

            if (activePeriod != null)
            {
                var result = new ResultDto<ReslutActiveStatisticalDto>()
                {
                    IsSuccess = true,
                    Message = "دوره آماری فعال با موفقیت دریافت شد.",
                    Data = new ReslutActiveStatisticalDto()
                    {
                        Id = activePeriod.Id,
                        StatisticalPeriod = activePeriod.StatisticalPeriod // مقدار مورد نظر دیگری که باید در ReslutActiveStatisticalDto قرار بگیرد
                    }
                };
                return result;
            }
            else
            {
                return new ResultDto<ReslutActiveStatisticalDto>()
                {
                    IsSuccess = false,
                    Message = "دوره آماری فعالی یافت نشد.",
                    Data = null // در این حالت می‌توانید Data را به null تنظیم کنید
                };
            }
        }

    }
    public class ReslutActiveStatisticalDto
    {
        public int Id { get; set; }
        public string StatisticalPeriod { get; set; }
    }

}
