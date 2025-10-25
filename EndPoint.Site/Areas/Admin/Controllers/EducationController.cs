using Azmoon.ElFinder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Nancy;
using System;
using System.IO;

using System.Threading.Tasks;


namespace EndPoint.Site.Areas.Admin.Controllers
    {
    [Area("Admin")]

    public class EducationController : Controller
        {
  
        public IActionResult Index()
            {
            return View();
            }
        }


    }
