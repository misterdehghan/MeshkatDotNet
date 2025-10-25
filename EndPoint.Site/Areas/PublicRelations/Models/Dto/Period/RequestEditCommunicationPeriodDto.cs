using System;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Period
{
    public class RequestEditCommunicationPeriodDto
    {
        public int Id { get; set; }

        public string StatisticalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
