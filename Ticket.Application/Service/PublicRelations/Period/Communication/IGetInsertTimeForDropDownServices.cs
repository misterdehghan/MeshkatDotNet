using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IGetInsertTimeForDropDownServices
    {
        ResultDto<List<ResultInsertTimeForDropDown>> Execute(int year);
    }

    public class ResultInsertTimeForDropDown
    {
        public int Id { get; set; }
        public string StatisticalPeriod { get; set; }
    }

    public class GetInsertTimeForDropDownServices : IGetInsertTimeForDropDownServices
    {
        private readonly IDataBaseContext _context;
        public GetInsertTimeForDropDownServices(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<ResultInsertTimeForDropDown>> Execute(int year)
        {
            // دریافت داده‌ها از دیتابیس
            var result = _context.CommunicationPeriod
                .Where(p => p.IsRemoved == false)
                .Where(p => p.InsertTime.Year == year).ToList()
                .Select(x => new ResultInsertTimeForDropDown
                {
                    StatisticalPeriod = x.StatisticalPeriod,
                    Id = x.Id
                }).ToList();

            // بسته‌بندی نتیجه در ResultDto
            return new ResultDto<List<ResultInsertTimeForDropDown>>
            {
                Data = result,
                IsSuccess = true,
                Message = "Data retrieved successfully"
            };
        }
    }

}
