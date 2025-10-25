using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IGetYearsForDropDownServices
    {
        ResultDto<List<int>> Execute();
    }

    public class GetYearsForDropDownServices : IGetYearsForDropDownServices
    {
        private readonly IDataBaseContext _context;


        public GetYearsForDropDownServices(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<int>> Execute()
        {
            // استخراج سال‌های یکتا از InsertTime
            var years = _context.CommunicationPeriod
                .Where(p => p.IsRemoved == false)
                .Select(n => n.InsertTime.Year) // انتخاب سال از InsertTime
                .Distinct() // سال‌های یکتا
                .OrderBy(year => year) // ترتیب‌گذاری بر اساس سال
                .ToList();

            // بسته‌بندی نتیجه در ResultDto
            return new ResultDto<List<int>>
            {
                Data = years,
                IsSuccess = true,
                Message = "Years retrieved successfully"
            };
        }
    }
}
