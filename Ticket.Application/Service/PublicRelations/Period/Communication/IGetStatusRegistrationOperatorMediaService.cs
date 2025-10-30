using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Period.Communication
{
    public interface IGetStatusRegistrationOperatorMediaService
    {
        ResultDto<List<ResultStatusRegistrationMedia>> Execute(RequestStatusRegistrationMedia request);
    }

    public class ResultStatusRegistrationMedia
    {
        public bool IsRegistered { get; set; }
        public string Operator { get; set; }
        // وارد شده ها
        public int RegisteredNumber { get; set; }
        // تایید شده ها
        public int ConfirmedNumber { get; set; }
        // تایید نشده ها
        public int UnconfirmedNumber { get; set; }
    }

    public class RequestStatusRegistrationMedia
    {
        public int CommunicationPeriodId { set; get; }
    }

    public class GetStatusRegistrationOperatorMediaService : IGetStatusRegistrationOperatorMediaService
    {
        private readonly IDataBaseContext _context;
        public GetStatusRegistrationOperatorMediaService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<ResultStatusRegistrationMedia>> Execute(RequestStatusRegistrationMedia request)
        {
            // گرفتن اپراتورها و بررسی اینکه آیا در عملکردهای دوره حضور دارند یا خیر
            var result = _context.Operators
                .OrderBy(o => o.InsertTime)
                .Select(o => new ResultStatusRegistrationMedia
                {
                    Operator = o.Name,
                    IsRegistered = _context.MediaPerformances.Where(o => o.IsRemoved == false)
                        .Any(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId),
                    RegisteredNumber = _context.MediaPerformances.Where(o => o.IsRemoved == false)
                        .Count(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId),
                    ConfirmedNumber = _context.MediaPerformances.Where(o => o.IsRemoved == false)
                        .Count(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId && p.Confirmation == true),
                    UnconfirmedNumber = _context.MediaPerformances.Where(o => o.IsRemoved == false)
                        .Count(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId && p.Confirmation == false)
                })
                .ToList();

            return new ResultDto<List<ResultStatusRegistrationMedia>>
            {
                IsSuccess = true,
                Data = result
            };

        }
    }

}
