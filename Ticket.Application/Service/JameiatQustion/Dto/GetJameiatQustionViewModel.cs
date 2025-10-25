using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.JameiatQustion.Dto
    {
    public class GetJameiatQustionViewModel
        {
        public int Id { get; set; }
        [Display(Name = "عنوان ")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        [Display(Name = "نوع")]
        public int typeQA { get; set; }
        [Display(Name = "وزن ")]
        public int Wight { get; set; }
        [Display(Name = "وضعیت ")]
        public bool IsChailren { get; set; } = false;
        }
    }
