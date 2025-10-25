using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IGetDataForViewBagMediaPerformancesService
    {
        ResultDto<ResultDataForViewBagMedia> Execute(RequestDataForViewBagMedia request);
    }

    public class ResultDataForViewBagMedia
    {
        public string TotalPlayingTime { get; set; }
        public int NumberOfRecords { get; set; }
        public int Confirmed { get; set; }
        public int Television { get; set; }
        public int Radio { get; set; }
    }


    public class RequestDataForViewBagMedia
    {
        public int CommunicationPeriodId { get; set; }
        public string NormalizedName { get; set; }

    }

    public class GetDataForViewBagMediaPerformancesService : IGetDataForViewBagMediaPerformancesService
    {
        private readonly IDataBaseContext _context;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

        public GetDataForViewBagMediaPerformancesService(IDataBaseContext context, IGetNameByNormalizedNameService getNameByNormalizedNameService)
        {
            _context = context;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
        }
        public ResultDto<ResultDataForViewBagMedia> Execute(RequestDataForViewBagMedia request)
        {
            double totalSeconds;
            int numberOfRecords;
            int confirmed;
            int television;
            int radio;
            if (request.NormalizedName == "Senior")
            {
                totalSeconds = _context.MediaPerformances
                    .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Confirmation == true)
                    .AsEnumerable() // تغییر داده‌ها به سمت کلاینت
                    .Sum(p => p.Time.TotalSeconds);

                numberOfRecords = _context.MediaPerformances
                    .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false)
                    .Count();

                confirmed = _context.MediaPerformances
                    .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Confirmation == true)
                    .Count();

                television= _context.MediaPerformances
                    .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Media == "تلوزیون")
                    .Count();

                radio = _context.MediaPerformances
                   .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Media == "رادیو")
                   .Count();
            }
            else
            {
                var operatorName = _getNameByNormalizedNameService.Execute(request.NormalizedName).Data;
                totalSeconds = _context.MediaPerformances
                    .Where(p => p.Operator == operatorName && p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Confirmation == true)
                    .AsEnumerable() // تغییر داده‌ها به سمت کلاینت
                    .Sum(p => p.Time.TotalSeconds);

                numberOfRecords = _context.MediaPerformances
                    .Where(p => p.Operator == operatorName && p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false)
                    .Count();

                confirmed = _context.MediaPerformances
                 .Where(p => p.Operator == operatorName && p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Confirmation == true)
                 .Count();

                television = _context.MediaPerformances
               .Where(p => p.Operator == operatorName &&  p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Media == "تلوزیون")
               .Count();

                radio = _context.MediaPerformances
                   .Where(p => p.Operator == operatorName &&  p.CommunicationPeriodId == request.CommunicationPeriodId && p.IsRemoved == false && p.Media == "رادیو")
                   .Count();
            }
            // تبدیل ثانیه‌ها به فرمت ساعت، دقیقه و ثانیه
            TimeSpan totalTimeSpan = TimeSpan.FromSeconds(totalSeconds);
            string formattedTime = $"{(int)totalTimeSpan.TotalHours} ساعت و {totalTimeSpan.Minutes} دقیقه و {totalTimeSpan.Seconds} ثانیه";

            var result = new ResultDataForViewBagMedia
            {
                TotalPlayingTime = formattedTime,
                NumberOfRecords = numberOfRecords,
                Confirmed = confirmed,
                Television=television,
                Radio=radio
            };




            return new ResultDto<ResultDataForViewBagMedia>
            {
                Data = result,
                IsSuccess = true,
                Message = "مجموع زمان‌ها محاسبه شد"
            };
        }
    }
}
