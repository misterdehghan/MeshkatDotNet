using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.Period.VirtualSpace
{
    public interface IGetListVirtualSpacePeriodService
    {
        ResultDto<ReslutPeriodDto> Execute(RequestPeriodDto request);
    }

    public class GetListVirtualSpacePeriodService : IGetListVirtualSpacePeriodService
    {
        private readonly IDataBaseContext _context;

        public GetListVirtualSpacePeriodService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ReslutPeriodDto> Execute(RequestPeriodDto request)
        {
            var statistical = _context.VirtualSpacePeriod.AsQueryable().Where(p => p.IsRemoved == false);
            if (!string.IsNullOrEmpty(request.searchKey))
            {
                statistical = statistical.Where(p => p.StatisticalPeriod.Contains(request.searchKey));
            }
            int rowsCount = 0;
            var StatisticalPeriod = statistical.ToPaged(request.page, request.pageSize, out rowsCount)
                .OrderByDescending(p => p.InsertTime)
                .Select(p => new GetPeriod
                {
                    Id = p.Id,
                    StatisticalPeriod = p.StatisticalPeriod,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    IsActive = p.IsActive
                }).ToList();
            return new ResultDto<ReslutPeriodDto>
            {
                Data = new ReslutPeriodDto
                {
                    getPeriods = StatisticalPeriod,
                    RowCount = rowsCount,
                    CurrentPage = request.page,
                    PageSize = request.pageSize
                },
                IsSuccess = true,
                Message = ""
            };
        }
    }

    public class ReslutPeriodDto
    {
        public List<GetPeriod> getPeriods { get; set; }


        //Pageination
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class GetPeriod
    {
        public int Id { get; set; }
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }


    public class RequestPeriodDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string searchKey { get; set; }
    }
}
