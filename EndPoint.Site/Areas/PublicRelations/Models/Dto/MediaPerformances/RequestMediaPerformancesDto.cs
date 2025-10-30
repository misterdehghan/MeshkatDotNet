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
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید")]
        public string Description { get; set; }
        [Required(ErrorMessage = "لطفا ساعت پخش برنامه را وارد کنید")]
        public TimeSpan BroadcastStartTime { get; set; } //ساعت پخش
        [Required(ErrorMessage = "لطفا تاریخ برنامه را وارد کنید")]
        public DateTime BroadcastDate { get; set; }
        [Required(ErrorMessage = "لطفا زمان برنامه را وارد کنید")]
        public TimeSpan Time { get; set; }
        [Required(ErrorMessage = "لطفا یک عکس به عنوان مستند وارد کنید")]
        public IFormFile Image { get; set; }

        public string Operator { get; set; }
    }
}
