using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.OperatorServices
{
    public interface IGetOperatorListServices
    {
        ResultDto<List<OperatorListDto>> Execute();
    }

    public class OperatorListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GetOperatorListServices : IGetOperatorListServices
    {
        private readonly IDataBaseContext _context;

        public GetOperatorListServices(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<OperatorListDto>> Execute()
        {

            var operatorName = _context.Operators
                .ToList()
                .Select(p => new OperatorListDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();

            return new ResultDto<List<OperatorListDto>>()
            {
                Data = operatorName,
                IsSuccess = true,
                Message = "",
            };
        }

    }
}
