using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class GetTemplateMainPagination
        {
        public List<GetTemplatesDto> Templates { get; set; }
        public PagerDto PagerDto { get; set; }
        }
    }
