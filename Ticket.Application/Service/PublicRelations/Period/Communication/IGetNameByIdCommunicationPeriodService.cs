using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IGetNameByIdCommunicationPeriodService
    {
        ResultDto<string> Execute(int id);
    }

    public class GetNameByIdCommunicationPeriodService : IGetNameByIdCommunicationPeriodService
    {
        private readonly IDataBaseContext _context;

        public GetNameByIdCommunicationPeriodService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<string> Execute(int id)
        {
            var result = _context.CommunicationPeriod.FirstOrDefault(p => p.Id == id).StatisticalPeriod;
            return new ResultDto<string>
            {
                IsSuccess = true,
                Data = result,
                Message = ""
            };
        }
    }
}
