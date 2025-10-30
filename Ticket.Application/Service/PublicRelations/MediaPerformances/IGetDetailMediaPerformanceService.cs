using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IGetDetailMediaPerformanceService
    {
        ResultDto<ResultDetailMedia> Execute(int id);
    }

    public class ResultDetailMedia
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public string Media { get; set; }
        public string NetworkName { get; set; }
        public string ProgramName { get; set; }
        public string SubjectTitle { get; set; }
        public string Description { get; set; }
        public TimeSpan BroadcastStartTime { get; set; }
        public DateTime BroadcastDate { get; set; }
        public TimeSpan Time { get; set; }
        public string Image { get; set; }
        public bool Confirmation { get; set; }
        public string CommunicationPeriod { set; get; }
        public string Operator { get; set; }
    }

    public class GetDetailMediaPerformanceService : IGetDetailMediaPerformanceService
    {
        private readonly IDataBaseContext _context;

        public GetDetailMediaPerformanceService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultDetailMedia> Execute(int id)
        {
            var mediaPerformance = _context.MediaPerformances.FirstOrDefault(p => p.Id == id);
            if (mediaPerformance == null)
            {
                return new ResultDto<ResultDetailMedia>
                {
                    IsSuccess = false,
                    Message = "رکورد یافت نشد."
                };
            }

            var communicationPeriod = _context.CommunicationPeriod.FirstOrDefault(p => p.Id == mediaPerformance.CommunicationPeriodId).StatisticalPeriod;

            var resultDetail = new ResultDetailMedia
            {
                Id = mediaPerformance.Id,
                InsertTime = mediaPerformance.InsertTime,
                UpdateTime = mediaPerformance.UpdateTime,

                Media = mediaPerformance.Media,
                NetworkName = mediaPerformance.NetworkName,
                ProgramName = mediaPerformance.ProgramName,
                SubjectTitle = mediaPerformance.SubjectTitle,
                Description=mediaPerformance.Description,
                BroadcastStartTime=mediaPerformance.BroadcastStartTime,
                BroadcastDate = mediaPerformance.BroadcastDate,
                Time = mediaPerformance.Time,
                Image = mediaPerformance.Image,
                Confirmation = mediaPerformance.Confirmation,
                CommunicationPeriod = communicationPeriod,
                Operator = mediaPerformance.Operator
            };

            return new ResultDto<ResultDetailMedia>
            {
                IsSuccess = true,
                Message = "عملیات با موفقیت انجام شد",
                Data = resultDetail
            };

        }
    }

}
