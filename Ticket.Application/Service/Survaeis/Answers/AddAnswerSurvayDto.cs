using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Answers
{
    public class AddAnswerSurvayDto
    {
        public long Id { get; set; }
        public long SurveyQuestionId { get; set; }
        public long SurveyId { get; set; }
        public int Wight { get; set; }
        public string Title { get; set; }
        public int Index { get; set; } = 1;
    }
}
