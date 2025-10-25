using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.MediaPerformances
{
    public class RequestMediaPerformancesDto
    {
        [Required(ErrorMessage = "لطفا نام رسانه را وارد کنید")]
        public string Media { get; set; }
        [Required(ErrorMessage = "لطفا نام شبکه را وارد کنید")]
        public string NetworkName { get; set; }
        [Required(ErrorMessage = "لطفا نام برنامه را وارد کنید")]
        public string ProgramName { get; set; }
        [Required(ErrorMessage = "لطفا موضوع برنامه را وارد کنید")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "لطفا تاریخ برنامه را وارد کنید")]
        public DateTime BroadcastDate { get; set; }
        [Required(ErrorMessage = "لطفا زمان برنامه را وارد کنید")]
        public TimeSpan Time { get; set; }
        [Required(ErrorMessage = "لطفا یک عکس به عنوان مستند وارد کنید")]
        public IFormFile Image { get; set; }

        public string Operator { get; set; }
    }
}
