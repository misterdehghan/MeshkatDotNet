using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Template
    {
    public class UserAccess
        {
        public int Id { get; set; }
        public int SystemType { get; set; } = 1;
        public long WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }
        public string UserName { get; set; }
        public string CreatorUserName { get; set; }
        public byte Status { get; set; } = 1;
        public DateTime RegesterAt { get; set; } = DateTime.Now;

        }
    }
