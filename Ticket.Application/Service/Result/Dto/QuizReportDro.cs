using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Result.Dto
{
   public class QuizReportDro
    {
        public long Id { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; } 
        [Display(Name = "کدملی")]
        public string melli { get; set; }
        [Display(Name = "تاریخ آزمون")]
        public string QuizStart { get; set; }
        [Display(Name = "شماره تماس")]
        public string Phone { get; set; }
        [Display(Name = "محل خدمت")]
        public string WorkPlaceName { get; set; }
        [Display(Name = "نمره")]
        public int Point { get; set; }
        [Display(Name = "درصد جواب صحیح")]
        public int Darsad { get; set; }
        public int counter { get; set; }
        public long? NumberBankAccunt { get; set; }
        }
}
