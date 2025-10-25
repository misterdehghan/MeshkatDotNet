using System;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.User
{
    public class GetUserDetailDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonnelCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Operator { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}

