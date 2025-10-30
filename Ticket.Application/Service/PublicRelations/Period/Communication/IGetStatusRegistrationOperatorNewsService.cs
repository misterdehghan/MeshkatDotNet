using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IGetStatusRegistrationOperatorNewsService
    {
        ResultDto<List<ResultStatusRegistration>> Execute(RequestStatusRegistration request);
    }

    public class ResultStatusRegistration
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

    public class RequestStatusRegistration
    {
        public int CommunicationPeriodId { set; get; }

    }

    public class GetStatusRegistrationOperatorNewsService : IGetStatusRegistrationOperatorNewsService
    {
        private readonly IDataBaseContext _context;

        public GetStatusRegistrationOperatorNewsService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<ResultStatusRegistration>> Execute(RequestStatusRegistration request)
        {
            // گرفتن اپراتورها و بررسی اینکه آیا در عملکردهای دوره حضور دارند یا خیر
            var result = _context.Operators
                .OrderBy(o => o.InsertTime)
                .Select(o => new ResultStatusRegistration
                {
                    Operator = o.Name,
                    IsRegistered = _context.NewsPerformances.Where(p=>p.IsRemoved==false)
                        .Any(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId),
                    RegisteredNumber = _context.NewsPerformances.Where(p => p.IsRemoved == false)
                        .Count(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId),
                    ConfirmedNumber = _context.NewsPerformances.Where(p => p.IsRemoved == false)
                        .Count(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId && p.Confirmation == true),
                    UnconfirmedNumber = _context.NewsPerformances.Where(p => p.IsRemoved == false)
                        .Count(p => p.Operator == o.Name && p.CommunicationPeriodId == request.CommunicationPeriodId && p.Confirmation == false)

                })
                .ToList();

            // بازگشت نتیجه
            return new ResultDto<List<ResultStatusRegistration>>
            {
                IsSuccess = true,
                Data = result
            };

        }
    }
}




