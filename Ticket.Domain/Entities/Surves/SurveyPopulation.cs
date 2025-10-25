using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Domain.Entities.Surves
{
  public  class SurveyPopulation : BaseEntity
    {
        public bool Gender { get; set; }
        public int KhedmatAgeRange { get; set; }
        public int TypeDarajeh { get; set; }
        public int darajeh { get; set; }
        public int Education { get; set; }
        public long? WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }
    }
}
