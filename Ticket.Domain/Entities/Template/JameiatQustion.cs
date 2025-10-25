using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Template
    {
    public class JameiatQustion
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public JameiatQustion Parent { get; set; }
        public int typeQA { get; set; }
        public int Wight { get; set; }
        public byte Status { get; set; } = 1;
        public DateTime RegesterAt { get; set; } = DateTime.Now;
        }
    }
