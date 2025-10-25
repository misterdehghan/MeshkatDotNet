using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Azmoon.Application.Service.Survaeis.Survayy
{
  public  class AddSurvayDto
    {
        public long Id { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string Name { get; set; }
        [Display(Name = "تعداد دفعات ثبت برای هر آی پی  ")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public int? AllowCountPerIp { get; set; } = 1;

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string Description { get; set; }
        [Display(Name = "دسته بندی ")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public long GroupId { get; set; }

        public string CreatorId { get; set; }
        public string UniqKey { get; set; }

        [Display(Name = "تاریخ و زمان شروع")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ و زمان پایان")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public DateTime? EndDate { get; set; }

        public List<AddFeature_dto> Features { get; set; }
        public IEnumerable<SelectListItem> GroupSelectList { get; set; } 
    }
    public class AddFeature_dto
    {
        public string Title { get; set; }
        public int Wight { get; set; }
        public int Index { get; set; } = 1;

    }
    public class EditFeature_dto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Wight { get; set; }
        public int Index { get; set; } = 1;
        public long SurveyId { get; set; }
    }

}
