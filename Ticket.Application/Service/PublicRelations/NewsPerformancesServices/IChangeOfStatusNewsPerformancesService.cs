using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.NewsPerformancesServices
{
    public interface IChangeOfStatusNewsPerformancesService
    {
        ResultDto Execute(int id, string Operator);
    }

    public class ChangeOfStatusNewsPerformancesService : IChangeOfStatusNewsPerformancesService
    {
        private readonly IDataBaseContext _context;
        public ChangeOfStatusNewsPerformancesService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int id, string Operator)
        {
            var newsPerformances = _context.NewsPerformances.FirstOrDefault(p => p.Id == id);
            if (Operator != "Senior")
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما مجاز به تغییر وضعیت نمی باشید"
                };
            }
            if (newsPerformances == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رکورد یافت نشد"
                };
            }


            // تغییر وضعیت Confirmation به حالت مخالف
            newsPerformances.Confirmation = !newsPerformances.Confirmation;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "وضعیت تغییر یافت"
            };
        }
    }
}
