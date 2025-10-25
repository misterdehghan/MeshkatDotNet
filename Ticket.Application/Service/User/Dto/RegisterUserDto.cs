using Azmoon.Common.Useful;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Azmoon.Application.Service.User.Dto
{
  public  class RegisterUserDto
    {
        public string Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        [RegularExpression(@"^([\u0600-\u06FF]+\s?)+$", ErrorMessage = " {0}  را به فارسی وارد نمائید")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = " تعداد کارکتر مجاز برای {0} حداقل 3 و حداکثر 15 می باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        [RegularExpression(@"^([\u0600-\u06FF]+\s?)+$", ErrorMessage = " {0}  را به فارسی وارد نمائید")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = " تعداد کارکتر مجاز برای {0}  حداقل 3 و حداکثر 20 می باشد")]
        public string LastName { get; set; }

        [Display(Name = "نام پدر")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        [RegularExpression(@"^([\u0600-\u06FF]+\s?)+$", ErrorMessage = " {0}  را به فارسی وارد نمائید")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = " تعداد کارکتر مجاز برای {0} حداقل 3 و حداکثر 15 می باشد")]
        public string name_father { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        [MobliPhon]
        public string Phone { get; set; }

        [Display(Name = "درجه/رتبه")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public int darajeh { get; set; }

        [Display(Name = "نوع درجه")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public int TypeDarajeh { get; set; }

        [Display(Name = "تاریخ تولد")]

        public string tavalod { get; set; } = DateTime.Now.ToString();


        [Display(Name = "شماره پرسنلی")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        [RegularExpression(@"(^40)?(\d{9}$)$", ErrorMessage = " {0}  را به درستی وارد نمائید")]

        public long personeli { get; set; }

        [CodeMelli(ErrorMessage = " {0}  را به درستی وارد نمائید ")]
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string melli { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,30}$", ErrorMessage = "رمز باید شامل 8 کارکتر حروف بزرگ و عدد  و متا کارکتر باشد")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,30}$", ErrorMessage = "رمز باید حداقل 6 کارکتر شامل حروف  و عدد   باشد")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,30}$", ErrorMessage = "رمز باید  حداقل 6 کارکتر شامل حروف  و عدد   باشد")]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,30}$", ErrorMessage = "رمز باید شامل 8 کارکتر حروف بزرگ و عدد  و متا کارکتر باشد")]
        [Compare(nameof(Password), ErrorMessage = "پسورد مطابق با رمز قبلی نمی باشد")]
        public string RePassword { get; set; }


        [Display(Name = "محل خدمت")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public long WorkPlaceId { get; set; }

        public string WorkPlaceIdFake { get; set; } = "";

        [Display(Name = " شماره حساب بانک سپه(واریز جایزه)")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        [RegularExpression(@"(\d{10,16}$)$", ErrorMessage = " {0}  را عدد وارد نمائید")]
        public long NumberBankAccunt { get; set; }

        }
}
