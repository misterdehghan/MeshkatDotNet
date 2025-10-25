using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.OperatorServices
{
    public interface IGetNormalizedNameByNameService
    {
        ResultDto<string> Execute(string name);
    }

    public class GetNormalizedNameByNameService : IGetNormalizedNameByNameService
    {
        private readonly IDataBaseContext _context;

        public GetNormalizedNameByNameService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<string> Execute(string name)
        {
            var result = _context.Operators.FirstOrDefault(p => p.Name == name).NormalizedName;
            return new ResultDto<string>
            {
                Data = result,
                IsSuccess = true
            };
        }
    }
}
