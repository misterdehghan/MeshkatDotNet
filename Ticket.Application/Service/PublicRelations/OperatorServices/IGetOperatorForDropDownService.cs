using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.OperatorServices
{
    public interface IGetOperatorForDropDownService
    {
        ResultDto<List<OperatorForDropDownDto>> Execute();
    }

    public class GetOperatorForDropDownService : IGetOperatorForDropDownService
    {
        private readonly IDataBaseContext _context;
        public GetOperatorForDropDownService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<OperatorForDropDownDto>> Execute()
        {
            string optionToRemoveName = "Senior";
            var operatorsList = _context.Operators
                .Where(o => o.NormalizedName != optionToRemoveName)
                .ToList().
                Select(p => new OperatorForDropDownDto
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();
            return new ResultDto<List<OperatorForDropDownDto>>
            {
                IsSuccess = true,
                Data = operatorsList,
                Message = ""
            };
        }
    }

    public class OperatorForDropDownDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
