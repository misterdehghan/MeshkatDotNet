using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class GetTemplatesDto
        {
        public int Id { get; set; }
        [Display(Name = "عنوان کلیشه")]
        public string Name { get; set; }

        [Display(Name = "کاربرایجاد کننده ")]
        public string UserName { get; set; }
        }
    }

