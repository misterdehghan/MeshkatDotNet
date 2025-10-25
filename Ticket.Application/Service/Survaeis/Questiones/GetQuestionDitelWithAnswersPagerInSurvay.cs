using Azmoon.Application.Service.Survaeis.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Questiones
{
   public class GetQuestionDitelWithAnswersPagerInSurvay
    {
        public GetQuestionSurvayViewModel GetQuestionDto { get; set; }
        public List<GetAnswerInSurvayViewModel> getAnswers { get; set; }
    }
}
