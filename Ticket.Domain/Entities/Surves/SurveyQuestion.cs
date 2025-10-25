using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Surves
{
    public class SurveyQuestion : BaseEntity
    {
       public string Text { get; set; }
       public int QuestionType { get; set; }

   
        public long SurveyId { get; set; }
        public virtual Survey Survey { get; set; }

        public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; }
        //public virtual ICollection<SurveyAnswerDescriptive> SurveyAnswerDescriptives { get; set; }
    }
}
