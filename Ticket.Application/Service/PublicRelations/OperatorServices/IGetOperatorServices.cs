using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.OperatorServices
{
    public interface IGetOperatorServices
    {
        ResultDto<ReslutOperator> Execute(RequestOperatorDto request);

    }

    public class GetOperatorServices : IGetOperatorServices
    {
        private readonly IDataBaseContext _context;

        public GetOperatorServices(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ReslutOperator> Execute(RequestOperatorDto request)
        {
            var operaor = _context.Operators.AsQueryable();

            if (!string.IsNullOrEmpty(request.searchKey))
            {
                operaor = operaor.Where(p => p.Name.Contains(request.searchKey));
            }
            int rowsCount = 0;
            var operaorList = operaor.OrderBy(p => p.InsertTime)
                .ToPaged(request.page, request.pageSize, out rowsCount)
                .Select(p => new OperatorDto
                {
                    InsertTime = p.InsertTime,
                    UpdateTime = p.UpdateTime,
                    Name = p.Name,
                    NormalizedName = p.NormalizedName
                }).ToList();

            return new ResultDto<ReslutOperator>
            {
                Data = new ReslutOperator
                {
                    OperatorDtos = operaorList,

                    RowCount = rowsCount,
                    CurrentPage = request.page,
                    PageSize = request.pageSize
                },
                IsSuccess = true,
                Message = ""
            };

        }
    }

    public class ReslutOperator
    {
        public List<OperatorDto> OperatorDtos { get; set; }

        //Pageination
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

    }

    public class OperatorDto
    {
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }


        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }

    public class RequestOperatorDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string searchKey { get; set; }
    }
}
