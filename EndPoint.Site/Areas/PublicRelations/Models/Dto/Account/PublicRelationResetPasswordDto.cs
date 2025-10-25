using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Account
{
    public class PublicRelationResetPasswordDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
