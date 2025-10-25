using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Results
{
  public  class DataResultSurvayDto
    {
        public IEnumerable<KeyValuePair<string, string>> answer { get; set; }
        public IEnumerable<KeyValuePair<string, string>> deccriptionAnswers { get; set; }


        public long SurvayId { get; set; }
        public string username { get; set; }
        public string Ip { get; set; }
        public long WorkPlaceId { get; set; }
    }
}
