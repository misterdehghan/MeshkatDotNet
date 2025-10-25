using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.PublicRelations.Main;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IAddCommunicationPeriodService
    {
        ResultDto Execute(RequestStatisticalCourseNP requestStatistical);
    }

    public class RequestStatisticalCourseNP
    {
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AddCommunicationPeriodService : IAddCommunicationPeriodService
    {
        private readonly IDataBaseContext _context;
        public AddCommunicationPeriodService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestStatisticalCourseNP requestStatistical)
        {
            // چک کردن تکراری نبودن StatisticalPeriod
            bool isDuplicate = _context.CommunicationPeriod.Any(s =>
                s.StatisticalPeriod == requestStatistical.StatisticalPeriod);

            if (isDuplicate)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تداخل نام دوره با موارد موجود در دیتابیس وجود دارد."
                };
            }


            // چک کردن تاریخ شروع از تاریخ پایان
            if (requestStatistical.StartDate > requestStatistical.EndDate)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تاریخ شروع نمی‌تواند از تاریخ پایان بزرگتر باشد."
                };
            }

            // چک کردن تاریخ شروع و تاریخ پایان
            if (requestStatistical.StartDate == requestStatistical.EndDate)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تاریخ شروع نمی‌تواند با تاریخ پایان برابر باشد."
                };
            }


            // چک کردن تداخل بازه‌های زمانی
            bool hasOverlap = _context.CommunicationPeriod.Any(s =>
                !s.IsRemoved &&  // بررسی بازه‌های غیرحذف شده
                s.StartDate <= requestStatistical.EndDate &&
                s.EndDate >= requestStatistical.StartDate);

            if (hasOverlap)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تداخل بازه‌های زمانی با موارد موجود در دیتابیس وجود دارد."
                };
            }
            else
            {
                CommunicationPeriod courseOfNP = new CommunicationPeriod
                {
                    StatisticalPeriod = requestStatistical.StatisticalPeriod,
                    StartDate = requestStatistical.StartDate,
                    EndDate = requestStatistical.EndDate,
                    InsertTime = DateTime.Now
                };


                _context.CommunicationPeriod.Add(courseOfNP);
                _context.SaveChanges();


                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "با موفقیت ذخیره شد"
                };
            }
        }
    }

}
