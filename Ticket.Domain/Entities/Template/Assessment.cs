using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Template
    {
    public class Assessment : BaseEntity<int>
        {
        public int AllowCountPerIp { get; set; } = 1;
        public string Name { get; set; }
        public string Description { get; set; }
        public long WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }
        public int TemplateMainId { get; set; }
        public TemplateMain TemplateMain { get; set; }
        public string CreatorUserName { get; set; }
        public User Creator { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PeriodTeachers { get; set; }
        }
    }
