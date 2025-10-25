using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Answers
{
    public interface ICrudAnswerSurvay
    {
        ResultDto<AddAnswerSurvayDto> Add(AddAnswerSurvayDto dto);
        ResultDto Remove(long Id);
        ResultDto<AddAnswerSurvayDto> Edit(AddAnswerSurvayDto dto);
        ResultDto<AddAnswerSurvayDto> EditKeyAnswer(EditFeature_dto dto);
        ResultDto<List<EditFeature_dto>> FindAnswerKeyById(long Id);
        ResultDto<GetAnswerSurvayViewModel> FindById(int Id);
        PagingDto<List<GetAnswerSurvayViewModel>> GetListPageineted(int page, int pageSize, long surveyQuestionId);
    }
}
