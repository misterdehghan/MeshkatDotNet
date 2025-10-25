using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.UserAccess.Dto
    {
    public class AddUserAccessDto
        {
        public int Id { get; set; }
        public string WorkPlaceName { get; set; }
        public long WorkPlaceId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        }
    }
