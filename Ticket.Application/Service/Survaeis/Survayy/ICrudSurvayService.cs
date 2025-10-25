using Azmoon.Application.Service.Survaeis.SurveyGroups;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Survayy
{
    public interface ICrudSurvayService
    {
        ResultDto<AddSurvayDto> Add(AddSurvayDto dto , string username);
        ResultDto Remove(long Id);
        ResultDto<AddSurvayDto> Edit(AddSurvayDto dto, string username);
        ResultDto<GetSurvayDto> FindById(long Id);
        ResultDto<AddSurvayDto, List<EditFeature_dto>> FindByIdForEdit(long Id);
        PagingDto<List<GetSurvayDto>> GetListPageineted(int page, int pageSize , string q ,int status, string userName);
    }
}
