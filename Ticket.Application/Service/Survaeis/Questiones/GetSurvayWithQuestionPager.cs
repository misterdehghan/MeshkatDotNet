using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Questiones
{
   public class GetSurvayWithQuestionPager
    {
        public PagingDto<List<GetQuestionSurvayViewModel>> getQuestionWithPager { get; set; }
        public GetSurvayDto GetSurvayDetiles { get; set; }
    }
}
