﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Azmoon.Application.Service.Group.Dto
{
   public class CreateGroupDto
    {
        public long Id { get; set; }
        [Display(Name = "نام رده")]
        public string Name { get; set; }
        [Display(Name = "رده والد")]
        public long? ParentId { get; set; }
        public List<SelectListItem> Parentes { get; set; }


    }
}
