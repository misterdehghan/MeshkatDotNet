using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.QuizTemp.Dto
{
  public  class GetQuizTempDto
    {
        public long QuizId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int point { get; set; } = 0;
        public string massage { get; set; }
        public string Ip { get; set; }
        }
}
