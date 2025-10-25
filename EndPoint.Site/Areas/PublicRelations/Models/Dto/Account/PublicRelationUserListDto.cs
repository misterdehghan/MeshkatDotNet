using System.Collections.Generic;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Account
{
    public class PublicRelationUserListDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PersonnelCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Province { get; set; }
        public bool IsActive { get; set; }
        public string Operator { get; set; }
        public string Role { get; set; }
    }
}
