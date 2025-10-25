using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.NewsPerformancesServices
{
    public interface IGetDetailNewsPerformanceService
    {
        ResultDto<ResultDetailNews> Execute(int id);
    }

    public class ResultDetailNews
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }


        public string NewsAgencyName { get; set; }
        public string Subject { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Image { get; set; }
        public bool Confirmation { get; set; }
        public string CommunicationPeriod { set; get; }
        public string Operator { get; set; }
    }

    public class GetDetailNewsPerformanceService : IGetDetailNewsPerformanceService
    {
        private readonly IDataBaseContext _context;

        public GetDetailNewsPerformanceService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultDetailNews> Execute(int id)
        {
            var newsPerformance = _context.NewsPerformances.FirstOrDefault(p => p.Id == id);
            if (newsPerformance == null)
            {
                return new ResultDto<ResultDetailNews>
                {
                    IsSuccess = false,
                    Message = "رکورد یافت نشد."
                };
            }

            var communicationPeriod = _context.CommunicationPeriod.FirstOrDefault(p => p.Id == newsPerformance.CommunicationPeriodId).StatisticalPeriod;

            var resultDetail = new ResultDetailNews
            {
                Id = newsPerformance.Id,
                InsertTime = newsPerformance.InsertTime,
                UpdateTime = newsPerformance.UpdateTime,
                NewsAgencyName = newsPerformance.NewsAgencyName,
                Subject = newsPerformance.Subject,
                PublicationDate = newsPerformance.PublicationDate,
                Image = newsPerformance.Image,
                Confirmation = newsPerformance.Confirmation,
                CommunicationPeriod = communicationPeriod,
                Operator = newsPerformance.Operator
            };

            return new ResultDto<ResultDetailNews>
            {
                IsSuccess = true,
                Message = "عملیات با موفقیت انجام شد",
                Data = resultDetail
            };
        }
    }
}
