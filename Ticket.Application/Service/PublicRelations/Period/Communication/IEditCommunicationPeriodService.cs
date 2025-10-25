using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IEditCommunicationPeriodService
    {
        ResultDto Execute(RequestEditCommunicationPeriod request);
    }

    public class RequestEditCommunicationPeriod
    {
        public int Id { get; set; }

        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class EditCommunicationPeriodService : IEditCommunicationPeriodService
    {
        private readonly IDataBaseContext _context;
        public EditCommunicationPeriodService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEditCommunicationPeriod request)
        {
            if (string.IsNullOrWhiteSpace(request.StatisticalPeriod) || request.StartDate == default || request.EndDate == default)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "فیلدها نمی‌توانند خالی باشند."
                };
            }

            var selectedPeriod = _context.CommunicationPeriod.FirstOrDefault(p => p.Id == request.Id);
            if (selectedPeriod == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دوره ای با این مشخصات یافت نشد."
                };
            }
            else
            {
                if (selectedPeriod.StatisticalPeriod == request.StatisticalPeriod &&
                selectedPeriod.StartDate == request.StartDate &&
                selectedPeriod.EndDate == request.EndDate)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "شما هیچ تغییری ایجاد نکردید"
                    };
                }

                selectedPeriod.StatisticalPeriod = request.StatisticalPeriod;
                selectedPeriod.StartDate = request.StartDate;
                selectedPeriod.EndDate = request.EndDate;

                _context.CommunicationPeriod.Update(selectedPeriod);
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "دوره با موفقیت ویرایش شد."
                };
            }
        }
    }
}
