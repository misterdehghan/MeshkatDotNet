using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Application.Service.WorkPlace.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class AddAssessmentDto
        {
        public int Id { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string Name { get; set; }
        [Display(Name = "تعداد دفعات ثبت برای هر آی پی  ")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public int? AllowCountPerIp { get; set; } = 1;

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string Description { get; set; }
        [Display(Name = "کلیشه  ")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public int TemplateMainId { get; set; }

        public string CreatorUserName { get; set; }
        [Display(Name = "تاریخ و زمان شروع")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ و زمان پایان")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "محل برگزاری ارزیابی  ")]
        public long WorkPlaceId { get; set; }


        public string WorkPlaceIdFake { get; set; } = "";
        public IEnumerable<GetWorkPlaceViewModel> WPlist { get; set; }
        public List<SelectListItem> TemplateSelectList { get; set; }
        public List<AddModaresFeatureDto> AddModaresFeatures { get; set; }
        }
    public class AddModaresFeatureDto {
        public string Modares { get; set; }
        public string Title { get; set; }
        }

    }
