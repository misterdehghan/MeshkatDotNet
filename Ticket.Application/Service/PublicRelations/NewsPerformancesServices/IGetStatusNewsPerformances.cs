using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.NewsPerformancesServices
{
    public interface IGetStatusNewsPerformances
    {
        ResultDto<bool> Execute(int id);
    }

    public class GetStatusNewsPerformances : IGetStatusNewsPerformances
    {
        private readonly IDataBaseContext _context;

        public GetStatusNewsPerformances(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<bool> Execute(int id)
        {
            var newsPerformances = _context.NewsPerformances.FirstOrDefault(p => p.Id == id);
            if (newsPerformances == null)
            {
                return new ResultDto<bool>
                {
                    IsSuccess = false,
                    Message = "رکورد یافت نشد"
                };
            }
            return new ResultDto<bool>
            {
                IsSuccess = true,
                Message = "وضعیت رکورد یافت شد",
                Data = newsPerformances.Confirmation
            };
        }
    }
}
