using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Questiones
{
    public interface ICrudQuestionSurvay
    {
        ResultDto<AddQuestionSurvayDto> Add(AddQuestionSurvayDto dto);
        ResultDto Remove(long Id);
        ResultDto<AddQuestionSurvayDto> Edit(AddQuestionSurvayDto dto);
        ResultDto<GetQuestionSurvayViewModel> FindById(long Id);
        GetSurvayWithQuestionPager GetListPageineted(int page, int pageSize, long survayId);
        ResultDto<GetQuestionDitelWithAnswersPagerInSurvay> GetQuestionDetailsWithAnswersPager(long questionId);
    }
}
