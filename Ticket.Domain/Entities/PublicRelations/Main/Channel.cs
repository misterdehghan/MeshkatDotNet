using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.PublicRelations.Main
{
    public class Channel
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsRemoved { get; set; } = false;
        public DateTime? RemoveTime { get; set; }

        public string MessengersName { get; set; }
        public string ChannelName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
   
        public DateTime ActivationDate { get; set; }
        public string Operator { get; set; }

        public ICollection<MembersPeriod> MembersPeriods { get; set; }
    }
}
