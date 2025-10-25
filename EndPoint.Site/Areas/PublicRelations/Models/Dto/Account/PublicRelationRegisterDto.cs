using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Account
{
    public class PublicRelationRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PersonnelCode { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool IsActive { get; set; }


    }
}
