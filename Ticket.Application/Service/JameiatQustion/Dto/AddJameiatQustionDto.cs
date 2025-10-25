using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.JameiatQustion.Dto
    {
    public class AddJameiatQustionDto
        {
        public int Id { get; set; }
        [Display(Name = "عنوان ")]
        public string Name { get; set; }
        [Display(Name = "والد ")]
        public int? ParentId { get; set; }
        [Display(Name = "نوع ")]
        public int typeQA { get; set; }
        [Display(Name = "وزن ")]
        public int Wight { get; set; } = 0;
        public List<SelectListItem> Parentes { get; set; }
        public string referer { get; set; }
        }
    }
