using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Results
{
   public class AddResultSurvayDto
    {
        public long Id { get; set; }
        public int Points { get; set; }
        public int MaxPoints { get; set; }
        public string AnsweresInQuiz { get; set; }
        public string StudentId { get; set; }
        public string Ip { get; set; }
        public DateTime StartQuiz { get; set; }
        public DateTime? EndQuiz { get; set; }
        public int SurvayId { get; set; }
    }
}
