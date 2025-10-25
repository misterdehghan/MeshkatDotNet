using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment.Dto
    {
    public class UserAccessWorkPlacesTreeViewDto
        {
        public long wpId { get; set; }
        public List<Azmoon.Application.Service.WorkPlace.Dto.GetWorkPlaceViewModel>  getWorksPlaces { get; set; }
    }
    }
