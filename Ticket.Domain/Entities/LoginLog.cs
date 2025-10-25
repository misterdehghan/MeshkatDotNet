using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities
{
  public class LoginLog
    {
        [Key]
        public long Id { get; set; }
        public byte Status { get; set; } = 1;
        public string UserName { get; set; }
        public string Ip { get; set; }
        public DateTime? RegesterAt { get; set; } = DateTime.Now;
        }
}
