using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Interfaces.Assessment
    {
    public interface IGetAssessment
        {
        ResultDto<GetAssessmentPagination> GetSearchAssessment(string username, string qSearch, DateTime? StartDate, DateTime? EndDate, long WorkPlaceId = 0);
        ResultDto<GetAssessmentPagination> GetAssessmentPagination( string username ,int page , int PageSize, bool userIsAdmin, int templateId = 0, long worckplae = 0, string q = "");   
        ResultDto<List<GetTemplatesDto>> GetTemplates( string username);
        ResultDto<GetDitelesTemplateDto> GetViewTemplateQustionAnswers(int id, string username);
        ResultDto<int> EditTemplateQustionAnswers(EditTemplateQustionAnswersDto dto, string userName, string Name);
        ResultDto<GetDitelesTemplateDto> GetTemplateQustionAnswers(int id , string username);
        ResultDto<List<GetAssessmentDto>> GetAssessmentWpId(long wpId);
        }
    }
