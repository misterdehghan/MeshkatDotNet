using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IDeleteCommunicationPeriodService
    {
        ResultDto Execute(int id);
    }

    public class DeleteCommunicationPeriodService : IDeleteCommunicationPeriodService
    {
        private readonly IDataBaseContext _context;
        public DeleteCommunicationPeriodService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int id)
        {
            var period = _context.CommunicationPeriod.FirstOrDefault(p => p.Id == id);
            if (period == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دوره ای با این مشخصات یافت نشد."
                };
            }
            else
            {

                var newsPerformances = _context.NewsPerformances.Any(p => p.CommunicationPeriodId == id && p.IsRemoved == false);
                var mediaPerformances = _context.MediaPerformances.Any(p => p.CommunicationPeriodId == id && p.IsRemoved == false);

                if (newsPerformances || mediaPerformances)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "شما مجاز به حذف این دوره نیستید<br> ابتدا باید تمامی عملکرد های وابسته به این دوره را حذف کنید"
                    };
                }
                else
                {
                    period.IsRemoved = true;
                    period.RemoveTime = DateTime.Now;
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccess = true,
                        Message = "دوره با موفقیت حذف شد"
                    };
                }

            }
        }
    }
}


