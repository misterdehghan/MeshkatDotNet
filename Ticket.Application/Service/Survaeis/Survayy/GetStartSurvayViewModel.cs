using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Application.Service.Survaeis.Questiones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Survayy
{
   public class GetStartSurvayViewModel
    {
        [Display(Name = "شماره نظرسنجی")]
        public long SurvayId { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        public List<GetQuestionSurvay> getQuestion { get; set; }

        [Display(Name = "محل خدمت")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public long WorkPlaceId { get; set; }

        public string WorkPlaceIdFake { get; set; } = "";
    }

    public class GetQuestionSurvay {

        public long Id { get; set; }
        [Display(Name = "متن سوال")]
        public string Text { get; set; }
        public int QuestionType { get; set; }
        public List<GetAnswerSurvayViewModel> getAnsweres { get; set; }
    }
}
