using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Surves
{
   public class Survey : BaseEntity
    {
        public int? AllowCountPerIp { get; set; } = 1;
        public string Name { get; set; }
        public string Description { get; set; }
        public long GroupId { get; set; }
        public Group Group { get; set; }
        public string CreatorId { get; set; }
        public  User Creator { get; set; }
        public string UniqKey { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<SurveyQuestion> SurveyQuestions { get; set; }
        public ICollection<SurveyAnswer> SurveyAnswers { get; set; }
    }
}
