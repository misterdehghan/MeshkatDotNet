using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Surves
{
  public class SurvayResultAnswer
    {
        public long Id { get; set; }
        public long SurveyId { get; set; }
        public string SurveyAnswerQuestion { get; set; }
        public DateTime CreateAt { get; set; }
        public string UserName { get; set; }
        public string Ip { get; set; }
        public long? WorkPlaceSurveyId { get; set; }
        public WorkPlace WorkPlaceSurvey { get; set; }
    }
}
