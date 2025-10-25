using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.VirtualSpace
{
    public interface IGetDetailVirtualSpacePeriodService
    {
        ResultDto<ResultStatisticalPeriod> Execute(int PeriodId);
    }
    public class ResultStatisticalPeriod
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }


    public class GetDetailVirtualSpacePeriodService : IGetDetailVirtualSpacePeriodService
    {
        private readonly IDataBaseContext _context;
        public GetDetailVirtualSpacePeriodService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultStatisticalPeriod> Execute(int PeriodId)
        {
            var Period = _context.VirtualSpacePeriod.SingleOrDefault(s => s.Id == PeriodId);
            if (Period == null)
            {
                return new ResultDto<ResultStatisticalPeriod>
                {
                    IsSuccess = false,
                    Message = "دوره آماری مورد نظر یافت نشد",
                    Data = null
                };
            }

            var resultStatisticalPeriod = new ResultStatisticalPeriod
            {
                Id = Period.Id,
                InsertTime = Period.InsertTime,
                UpdateTime = Period.UpdateTime,
                StatisticalPeriod = Period.StatisticalPeriod,
                StartDate = Period.StartDate,
                EndDate = Period.EndDate,
                IsActive = Period.IsActive
            };


            return new ResultDto<ResultStatisticalPeriod>
            {
                IsSuccess = true,
                Message = "دوره آماری مورد نظر یافت شد",
                Data = resultStatisticalPeriod
            };
        }
    }
}
