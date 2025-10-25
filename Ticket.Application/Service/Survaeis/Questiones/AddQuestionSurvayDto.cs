using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Questiones
{
   public class AddQuestionSurvayDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public int QuestionType { get; set; }
        public long SurveyId { get; set; }
    }
}
