using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Account
{
    public class PublicRelationLoginDto
    {
        [Required]
        [Display(Name = "شماره پرسنلی")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember")]
        public bool IsPersistent { get; set; } = false;
         


    }
}
