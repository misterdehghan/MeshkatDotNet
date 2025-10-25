using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.VirtualSpace
{
    public interface IEditVirtualSpacePeriodService
    {
        ResultDto Execute(ForPeriodEdit request);
    }

    public class ForPeriodEdit
    {
        public int Id { get; set; }

        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class EditVirtualSpacePeriodService : IEditVirtualSpacePeriodService
    {
        private readonly IDataBaseContext _context;
        public EditVirtualSpacePeriodService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(ForPeriodEdit request)
        {
            if (string.IsNullOrWhiteSpace(request.StatisticalPeriod) || request.StartDate == default || request.EndDate == default)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "فیلدها نمی‌توانند خالی باشند."
                };
            }


            var selectedPeriod = _context.VirtualSpacePeriod.FirstOrDefault(p => p.Id == request.Id);
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

                _context.VirtualSpacePeriod.Update(selectedPeriod);
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "دوره با موفقیت ویرایش شد."
                };
            }

            //----------------

            //var period = _context.VirtualSpacePeriod.FirstOrDefault(p => p.Id == periodEdit.Id);

            //if (period == null)
            //{
            //    return new ResultDto
            //    {
            //        IsSuccess = false,
            //        Message = "دوره آماری مورد نظر یافت نشد"
            //    };
            //}

            //if (period != null)
            //{
            //    if (period.StatisticalPeriod != periodEdit.StatisticalPeriod)
            //    {
            //        bool isDuplicate = _context.VirtualSpacePeriod.Any(s =>
            //            s.StatisticalPeriod == periodEdit.StatisticalPeriod);

            //        if (isDuplicate)
            //        {
            //            return new ResultDto
            //            {
            //                IsSuccess = false,
            //                Message = "تداخل نام دوره با موارد موجود در دیتابیس وجود دارد."
            //            };
            //        }
            //        else
            //        {
            //            period.StatisticalPeriod = periodEdit.StatisticalPeriod;
            //        }

            //    }

            //    // چک کردن تاریخ شروع از تاریخ پایان
            //    if (periodEdit.StartDate > periodEdit.EndDate)
            //    {
            //        return new ResultDto
            //        {
            //            IsSuccess = false,
            //            Message = "تاریخ شروع نمی‌تواند از تاریخ پایان بزرگتر باشد."
            //        };
            //    }

            //    // چک کردن تاریخ شروع و تاریخ پایان
            //    if (periodEdit.StartDate == periodEdit.EndDate)
            //    {
            //        return new ResultDto
            //        {
            //            IsSuccess = false,
            //            Message = "تاریخ شروع نمی‌تواند با تاریخ پایان برابر باشد."
            //        };
            //    }

            //    // چک کردن تداخل بازه‌های زمانی
            //    bool hasOverlap = _context.VirtualSpacePeriod.Any(s =>
            //        !s.IsRemoved &&  // بررسی بازه‌های غیرحذف شده
            //        s.StartDate <= periodEdit.EndDate &&
            //        s.EndDate >= periodEdit.StartDate);

            //    if (hasOverlap)
            //    {
            //        return new ResultDto
            //        {
            //            IsSuccess = false,
            //            Message = "تداخل بازه‌های زمانی با موارد موجود در دیتابیس وجود دارد."
            //        };
            //    }
            //    else
            //    {
            //        if (period.StartDate != periodEdit.StartDate)
            //        {
            //            period.StartDate = periodEdit.StartDate;
            //        }

            //        if (period.EndDate != periodEdit.EndDate)
            //        {
            //            period.EndDate = periodEdit.EndDate;
            //        }
            //    }

            //    _context.VirtualSpacePeriod.Update(period);
            //    _context.SaveChanges();

            //    return new ResultDto()
            //    {
            //        IsSuccess = true,
            //        Message = "ویرایش با موفقیت انجام شد"
            //    };
            //}

            //return new ResultDto()
            //{
            //    IsSuccess = false,
            //    Message = "خطایی رخ داده است دوباره سعی کنید"
            //};


        }
    }
}


