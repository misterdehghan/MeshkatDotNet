using Azmoon.Application.Interfaces.Contexts;
using System;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.Period.Communication
{
    public interface IFindActiveCommunicationPeriodService
    {
        bool Execute();
    }

    public class FindActiveCommunicationPeriodService : IFindActiveCommunicationPeriodService
    {
        private readonly IDataBaseContext _context;

        public FindActiveCommunicationPeriodService(IDataBaseContext context)
        {
            _context = context;
        }

        public bool Execute()
        {
            // پیدا کردن دوره فعال (تنها یک دوره فعال وجود دارد)
            var activeStatistics = _context.CommunicationPeriod
                .Where(stat => stat.IsRemoved == false)
                .FirstOrDefault(stat => DateTime.Now >= stat.StartDate && DateTime.Now <= stat.EndDate);

            // چاپ مقادیر برای دیباگ
            if (activeStatistics != null)
            {
                return true;
            }

            return false;
        }
    }
}
