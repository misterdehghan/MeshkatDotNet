using System;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Statistics
{
    public class GetDeleteStatisticalPeriodDto
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
