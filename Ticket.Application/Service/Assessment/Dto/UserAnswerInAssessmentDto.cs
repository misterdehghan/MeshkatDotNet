using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class UserAnswerInAssessmentDto
        {
        public IEnumerable<KeyValuePair<string, string>> answer { get; set; }
        public IEnumerable<KeyValuePair<string, string>> jameiatanswer { get; set; }

        public IEnumerable<KeyValuePair<string, string>> modaresAnswers { get; set; }
        public int AssessmentId { get; set; }
        public string username { get; set; }
        public string deccriptionAnswers { get; set; }
        public string Ip { get; set; }
        public long WorkPlaceId { get; set; }
        }
    }
