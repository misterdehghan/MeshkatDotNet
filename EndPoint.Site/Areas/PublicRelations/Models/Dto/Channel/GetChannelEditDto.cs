using System;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Channel
{
    public class GetChannelEditDto
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
    }
}
