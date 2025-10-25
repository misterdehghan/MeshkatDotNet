using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.PublicRelations.Main
{
    public class MembersPeriod
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsRemoved { get; set; } = false;
        public DateTime? RemoveTime { get; set; }

        public int Member { get; set; }


        public Channel Channel { get; set; }
        public int ChannelId { get; set; }

        public VirtualSpacePeriod VirtualSpacePeriod { get; set; }
        public int VirtualSpacePeriodId { get; set; }
    }
}
