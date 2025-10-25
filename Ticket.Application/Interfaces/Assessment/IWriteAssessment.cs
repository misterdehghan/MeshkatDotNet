using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Interfaces.Assessment
    {
    public interface IWriteAssessment
        {
        ResultDto<AddAssessmentDto> GetAssessment(int id, string username);   
        ResultDto<int> AddAssessment(AddAssessmentDto dto, string username);  
        ResultDto<int> Add(AddTemplateDto dto, string username);
        ResultDto<int> Delete(int id);
        ResultDto<int> DeleteAssessment(int id);
        }
    }
