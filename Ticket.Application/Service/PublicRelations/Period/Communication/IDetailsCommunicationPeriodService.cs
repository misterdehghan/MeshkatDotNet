using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IDetailsCommunicationPeriodService
    {
        ResultDto<ReslutDetailsCommunication> Execute(int id);
    }

    public class ReslutDetailsCommunication
    {
        public int Id { get; set; }
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DetailsCommunicationPeriodService : IDetailsCommunicationPeriodService
    {
        private readonly IDataBaseContext _context;
        public DetailsCommunicationPeriodService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ReslutDetailsCommunication> Execute(int id)
        {
            var result = _context.CommunicationPeriod.FirstOrDefault(p => p.Id == id);
            if (result != null)
            {
                var details = new ReslutDetailsCommunication
                {
                    Id = id,
                    StatisticalPeriod = result.StatisticalPeriod,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate
                };

                return new ResultDto<ReslutDetailsCommunication>
                {
                    IsSuccess = true,
                    Message = "دوره پیدا شد",
                    Data = details
                };
            }
            else
            {
                return new ResultDto<ReslutDetailsCommunication>
                {
                    IsSuccess = false,
                    Message = "دوره پیدا نشد",
                };
            }
        }
    }
}
