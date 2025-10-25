using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IGetListCommunicationPeriodService
    {
        ResultDto<ReslutForPeriodNPDto> Execute(RequestPeriodNPDto request);
    }

    public class ReslutForPeriodNPDto
    {
        public List<PeriodNP> GetPeriodNPs { get; set; }

        //Pageination
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
    public class RequestPeriodNPDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string searchKey { get; set; }
    }

    public class PeriodNP
    {
        public int Id { get; set; }
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetListCommunicationPeriodService : IGetListCommunicationPeriodService
    {
        private readonly IDataBaseContext _context;
        public GetListCommunicationPeriodService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ReslutForPeriodNPDto> Execute(RequestPeriodNPDto request)
        {
            var statisticalNP = _context.CommunicationPeriod.AsQueryable().Where(p => p.IsRemoved == false);
            if (!string.IsNullOrEmpty(request.searchKey))
            {
                statisticalNP = statisticalNP.Where(p => p.StatisticalPeriod.Contains(request.searchKey));
            }
            int rowsCount = 0;


            var StatisticalPeriodNP = statisticalNP.ToPaged(request.page, request.pageSize, out rowsCount)
                .OrderByDescending(p => p.InsertTime)
                .Select(p => new PeriodNP
                {
                    Id = p.Id,
                    StatisticalPeriod = p.StatisticalPeriod,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    IsActive = p.IsActive
                }).ToList();

            return new ResultDto<ReslutForPeriodNPDto>
            {
                Data = new ReslutForPeriodNPDto
                {
                    GetPeriodNPs = StatisticalPeriodNP,
                    RowCount = rowsCount,
                    CurrentPage = request.page,
                    PageSize = request.pageSize
                },
                IsSuccess = true,
                Message = ""
            };
        }
    }

}
