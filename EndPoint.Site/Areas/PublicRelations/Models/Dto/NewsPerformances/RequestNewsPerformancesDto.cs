using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.NewsPerformances
{
    public class RequestNewsPerformancesDto
    {
        [Required(ErrorMessage = "لطفا نام خبرگزاری را وارد کنید")]
        public string NewsAgencyName { get; set; }
        [Required(ErrorMessage = "لطفا موضوع را وارد کنید")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "لطفا تاریخ انتشار را وارد کنید")]
        public DateTime PublicationDate { get; set; }
        [Required(ErrorMessage = "لطفا یک عکس به عنوان مستند وارد کنید")]
        public IFormFile Image { get; set; }
        public string Operator { get; set; }
    }
}
