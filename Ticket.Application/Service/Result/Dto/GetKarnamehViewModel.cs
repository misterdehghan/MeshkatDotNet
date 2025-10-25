using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Result.Dto
{
   public class GetKarnamehViewModel
    {
        public string questionText { get; set; }
        public string userAnswer { get; set; }
        public bool IsTrue { get; set; }
    }
}
