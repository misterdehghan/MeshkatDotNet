using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class GetAssessmentDto
        {
        public int Id { get; set; }
        [Display(Name = "نام")]

        public string Name { get; set; }
        [Display(Name = "تعداد دفعات ثبت برای هر آی پی  ")]

        public int? AllowCountPerIp { get; set; } = 1;

        [Display(Name = "توضیحات")]

        public string Description { get; set; }
        [Display(Name = "کلیشه  ")]
    
        public string TemplateMainName { get; set; }
        [Display(Name = "کاربر ایجاد کننده  ")]
        public string CreatorUserName { get; set; }
        [Display(Name = "تاریخ و زمان شروع")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ و زمان پایان")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "محل برگزاری ارزیابی  ")]

        public string WorkPlaceName { get; set; } = "";
        public int countUser { get; set; } = 0;
        public byte Status { get; set; } = 1;
        }
    }
