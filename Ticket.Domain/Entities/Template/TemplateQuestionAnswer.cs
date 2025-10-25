using Azmoon.Domain.Entities.Surves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Template
    {
    public class TemplateQuestionAnswer : BaseEntity<int>
        {
        public string Title { get; set; }
        public int QA_Type { get; set; }
        public int TemplateMainId { get; set; }
        public TemplateMain TemplateMain { get; set; }
        public int Wight { get; set; }
        }
    }
