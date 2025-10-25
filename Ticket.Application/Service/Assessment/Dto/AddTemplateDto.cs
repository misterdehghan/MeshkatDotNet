using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
{
    public class AddTemplateDto
    {
        public int Id { get; set; }
        [Display(Name = "عنوان کلیشه")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string Name { get; set; }
        public List<AddAnswerFeatureDto> AnswerFeatures { get; set; }
        public List<AddQustionFeatureDto> QuestionFeatures { get; set; }


    }

}
