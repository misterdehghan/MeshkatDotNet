using Azmoon.Application.Service.JameiatQustion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class GetDitelesTemplateDto
        {
        public int Id { get; set; }  
        public string Name { get; set; }
        public List<AddAnswerFeatureDto> AnswerFeatures { get; set; }
        public List<AddQustionFeatureDto> QuestionFeatures { get; set; }  
        public List<GetJameiatQustionViewModel> JameiatQustions { get; set; }
        public List<AddModaresFeatureDto> ModaresFeatures { get; set; }
        }
    }
