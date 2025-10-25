using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Questiones
{
  public  class GetAnswerWithPagerInSurvay
    {
        public PagerDto PagerDto { get; set; }
        public List<GetAnswerInSurvayViewModel> getAnswers { get; set; }
    }
}
