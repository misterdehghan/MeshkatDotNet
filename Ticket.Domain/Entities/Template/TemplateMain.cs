using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Template
    {
    public class TemplateMain : BaseEntity<int>
        {
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public User Creator { get; set; }
        }
    }
