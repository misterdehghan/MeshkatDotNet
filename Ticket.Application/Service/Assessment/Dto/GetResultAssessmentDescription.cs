using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
   

    public class GetResultAssessmentDescription
        {
        public int AssessmentId { get; set; }
        public string AssessmentTitle { get; set; }
        public List<ReportDescAssessmenDto> GetAnseres { get; set; }
        }
    public class ReportDescAssessmenDto
        {
        public string Text { get; set; }
        public string Ip { get; set; }
        public string WorkPlaceUser { get; set; }
        public string datetime { get; set; }
        }
    }
