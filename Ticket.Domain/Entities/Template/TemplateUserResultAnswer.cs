using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Template
    {
    public class TemplateUserResultAnswer
        {
        public long Id { get; set; }
        public int TemplateMainId { get; set; } 
        public int AssessmentId { get; set; }
        public string TemplateAnswerQuestion { get; set; }
        public string JaneiatAnswerQuestion { get; set; }
        public DateTime CreateAt { get; set; }
        public string UserName { get; set; }
        public string Ip { get; set; }
        public long? WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }
        public string ModaresAnswer { get; set; }
        }
    }
