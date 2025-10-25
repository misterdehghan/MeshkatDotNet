using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.PublicRelations.Main;
using System;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.Period.VirtualSpace
{
    public interface IAddVirtualSpacePeriodService
    {
        ResultDto Execute(ForAddStatisticalPeriodDto forAddStatisticalPeriod);
    }

    public class AddVirtualSpacePeriodService : IAddVirtualSpacePeriodService
    {
        private readonly IDataBaseContext _context;
        public AddVirtualSpacePeriodService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(ForAddStatisticalPeriodDto forAddStatisticalPeriod)
        {
            // چک کردن تکراری نبودن StatisticalPeriod
            bool isDuplicate = _context.VirtualSpacePeriod.Any(s =>
                s.StatisticalPeriod == forAddStatisticalPeriod.StatisticalPeriod);

            if (isDuplicate)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تداخل نام دوره با موارد موجود در دیتابیس وجود دارد."
                };
            }


            // چک کردن تاریخ شروع از تاریخ پایان
            if (forAddStatisticalPeriod.StartDate > forAddStatisticalPeriod.EndDate)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تاریخ شروع نمی‌تواند از تاریخ پایان بزرگتر باشد."
                };
            }

            // چک کردن تاریخ شروع و تاریخ پایان
            if (forAddStatisticalPeriod.StartDate == forAddStatisticalPeriod.EndDate)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تاریخ شروع نمی‌تواند با تاریخ پایان برابر باشد."
                };
            }


            // چک کردن تداخل بازه‌های زمانی
            bool hasOverlap = _context.VirtualSpacePeriod.Any(s =>
                !s.IsRemoved &&  // بررسی بازه‌های غیرحذف شده
                s.StartDate <= forAddStatisticalPeriod.EndDate &&
                s.EndDate >= forAddStatisticalPeriod.StartDate);

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
                VirtualSpacePeriod statistics = new VirtualSpacePeriod
                {
                    InsertTime = DateTime.Now,
                    IsRemoved = false,
                    StatisticalPeriod = forAddStatisticalPeriod.StatisticalPeriod,
                    StartDate = forAddStatisticalPeriod.StartDate,
                    EndDate = forAddStatisticalPeriod.EndDate
                };
                _context.VirtualSpacePeriod.Add(statistics);
                _context.SaveChanges();
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "با موفقیت ذخیره شد"
                };
            }
        }
    }

    public class ForAddStatisticalPeriodDto
    {
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
