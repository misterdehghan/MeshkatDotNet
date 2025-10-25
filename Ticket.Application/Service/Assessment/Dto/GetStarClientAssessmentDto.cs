using Azmoon.Application.Service.JameiatQustion.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class GetStarClientAssessmentDto
        {
        public int AssessmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GetJameiatQustionViewModel> getJameiatQustions { get; set; }
        public GetDitelesTemplateDto getQuestionAnswer { get; set; }
        public List<AddModaresFeatureDto> modaresFeatureDtos { get; set; }

    }
    }
