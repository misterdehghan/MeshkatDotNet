using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Roles
{
    public class GetAssignRoleToUserDto
    {
  
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonnelCode { get; set; }
        public string Operator { get; set; }
        public List<string> CurrentRole { get; set; }


        // اضافه کردن ویژگی برای ذخیره لیست نقش‌ها
        public IEnumerable<IdentityRole> Roles { get; set; }

    }
}
