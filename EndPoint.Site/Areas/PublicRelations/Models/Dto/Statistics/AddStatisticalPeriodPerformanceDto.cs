using System;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Statistics
{
    public class AddStatisticalPeriodPerformanceDto
    {
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
