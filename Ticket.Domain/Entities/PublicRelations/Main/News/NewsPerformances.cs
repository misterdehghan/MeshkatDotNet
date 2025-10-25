using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.PublicRelations.Main.News
{
    public class NewsPerformances
    {
        [Key]
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsRemoved { get; set; } = false;
        public DateTime? RemoveTime { get; set; }


        public string NewsAgencyName { get; set; }
        public string Subject { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Image { get; set; }
        public bool Confirmation { get; set; }



        [ForeignKey("CommunicationPeriodId")]
        public CommunicationPeriod CommunicationPeriod { get; set; }
        public int CommunicationPeriodId {  set; get; }  


        public string Operator { get; set; }
    }
}
