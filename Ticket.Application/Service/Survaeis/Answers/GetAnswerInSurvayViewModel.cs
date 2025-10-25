using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Answers
{
   public class GetAnswerInSurvayViewModel
    {
        public long Id { get; set; }
        public long SurveyQuestionId { get; set; }
        [Display(Name = "وزن")]
        public int Wight { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "اولویت نمایش")]
        public int Index { get; set; }
    }
}
