using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class AddAnswerFeatureDto
        {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string Title { get; set; }
        public int Wight { get; set; }
        public int Index { get; set; } = 1;
        }
    }
