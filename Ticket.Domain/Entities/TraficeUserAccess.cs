using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities
{
    public class TraficeUserAccess
        {
        public long Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public long WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }

        public ApprovalLevel Level { get; set; }
        }

    public enum ApprovalLevel
        {
        Country = 0,
        Province = 1,
        City = 2
        }
    }
