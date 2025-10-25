using System;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Models
    {
    public class UserShowPersonDto
        {
        public string Id { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string LastName { get; set; }
        [Display(Name = "شماره پرسنلی")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]

        public string UserName { get; set; }
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = " {0}  را وارد نمائید ")]
        public string Phone { get; set; }

        [Display(Name = "محل خدمت")]
        public string WorkPlaceName { get; set; }
        }
    public class UserShowPersonDtoLogin
        {
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "شماره پرسنلی")]
        public string UserName { get; set; }

        public string Phone { get; set; }
        public string Ip { get; set; }

        // تغییر نام به RegisteredAt
        public DateTime? RegesterAt { get; set; } = DateTime.Now;

        [Display(Name = "محل خدمت")]
        public string WorkPlaceName { get; set; }
        }
    }
