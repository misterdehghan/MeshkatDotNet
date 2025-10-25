using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Surves
{
    public class SurveyAnswerDescriptive : BaseEntity
    {
        public long SurveyId { get; set; }
        public long SurveyQuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string Ip { get; set; }
    }
}
