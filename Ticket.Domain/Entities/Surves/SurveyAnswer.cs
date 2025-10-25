using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Surves
{
    public class SurveyAnswer : BaseEntity
    {
        public long SurveyId { get; set; }
        public Survey Survey { get; set; }
        public long? SurveyQuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
        public int Wight { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
    }
}
