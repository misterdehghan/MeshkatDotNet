using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Surves
{
   public class Vote
    {
        public long Id { get; set; }
        public long SurveyQuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
        public long SurveyAnswerId { get; set; }
        public SurveyAnswer SurveyAnswer { get; set; }
        public long SurveyId { get; set; }
        public  Survey Survey { get; set; }
        public DateTime RegesterAt { get; set; } = DateTime.Now;
    }
}
