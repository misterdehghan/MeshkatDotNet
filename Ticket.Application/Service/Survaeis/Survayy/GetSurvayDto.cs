using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Survayy
{
  public  class GetSurvayDto
    {
        [Display(Name = "شماره نظرسنجی")]
        public long Id { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "دسته بندی")]
        public string GroupName { get; set; }

        [Display(Name = "تاریخ و زمان شروع")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ و زمان پایان")]
        public DateTime EndDate { get; set; }

        [Display(Name = "وضعیت")]
        public byte Status { get; set; }

        [Display(Name = "فیلتر")]
        public bool FilterStatus { get; set; }

        [Display(Name = "وضعیت")]
        public int state { get; set; }
        [Display(Name = "تعداد شرکت کنندگان")]
        public int CountResoult { get; set; }
        public string UniqKey { get; set; }
        }
}
