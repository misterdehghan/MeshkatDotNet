using System;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Channel
{
    public class AddChannlDto
    {
        [Required]
        public string MessengersName { get; set; }
        [Required]
        public string ChannelName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Member { get; set; }

        public DateTime ActivationDate { get; set; }
        public string Operator { get; set; }
    }
}
