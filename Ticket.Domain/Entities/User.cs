using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string melli { get; set; }
        public string name_father { get; set; }
        public string tavalod { get; set; }
        public long? GroupId { get; set; }
        public Group Group { get; set; }
        public int TypeDarajeh { get; set; }
        public int darajeh { get; set; }
        public long? NumberBankAccunt { get; set; }
        public long? WorkPlaceId { get; set; }
        public  WorkPlace WorkPlace { get; set; }

        #region PublicRelations

        public string Province { get; set; }
        public string City { get; set; }
        public string Operator { get; set; }
        public bool IsActive { get; set; } = false;

        #endregion

        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }

    }

}
