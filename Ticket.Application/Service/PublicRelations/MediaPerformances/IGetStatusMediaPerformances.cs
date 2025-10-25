
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IGetStatusMediaPerformances
    {
        ResultDto<bool> Execute(int id);
    }

    public class GetStatusMediaPerformances : IGetStatusMediaPerformances
    {
        private readonly IDataBaseContext _context;

        public GetStatusMediaPerformances(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<bool> Execute(int id)
        {
            var mediaPerformances = _context.MediaPerformances.FirstOrDefault(p => p.Id == id);
            if (mediaPerformances == null)
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
                Data = mediaPerformances.Confirmation
            };
        }
    }

}
