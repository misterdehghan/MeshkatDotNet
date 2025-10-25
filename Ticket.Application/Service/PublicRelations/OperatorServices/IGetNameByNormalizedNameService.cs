using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.OperatorServices
    {
    public interface IGetNameByNormalizedNameService
        {
        ResultDto<string> Execute(string normalizedName);
        }

    public class GetNameByNormalizedNameService : IGetNameByNormalizedNameService
        {
        private readonly IDataBaseContext _context;
        public GetNameByNormalizedNameService(IDataBaseContext context)
            {
            _context = context;
            }
        public ResultDto<string> Execute(string normalizedName)
            {
            if (normalizedName == null)
                {
                return new ResultDto<string>
                    {
                    Data = "بدون انتخاب",
                    IsSuccess = false
                    };
                }
            else
                {
                var r = _context.Operators.FirstOrDefault(p => p.NormalizedName == normalizedName);

                return new ResultDto<string>
                    {
                    Data = r != null ? r.Name : "یافت نشد",
                    IsSuccess = r != null
                    };
                }
            }
        }
    }
