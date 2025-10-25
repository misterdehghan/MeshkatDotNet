using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Questiones
{
  public class GetQuestionSurvayViewModel
    {
        public long Id { get; set; }
        [Display(Name = "متن سوال")]
        public string Text { get; set; }

        public long SurveyId { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        [Display(Name = "نوع سوال")]
        public int QuestionType { get; set; }
        [Display(Name = "تعداد جواب  ")]
        public int CountAsnswer { get; set; }

    }
}
