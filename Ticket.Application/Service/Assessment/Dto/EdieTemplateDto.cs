using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class EditTemplateQustionAnswersDto
        {
        public int TemplateId { get; set; }
        public List<KeyValuePair<string, string>> answerFeaturTitle   {get;set;}
        public List<KeyValuePair<string, string>> answerFeaturWight   {get;set;}
        public List<KeyValuePair<string, string>> QuestionFeaturTitle { get; set; }
        }
    }
