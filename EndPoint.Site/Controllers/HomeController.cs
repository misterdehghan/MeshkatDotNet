using EndPoint.Site.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azmoon.Application.Interfaces;
using Azmoon.Application.Service.Role.Dto;
using FastReport.Web;
using EndPoint.Site.Useful.Static;
using System.IO;
using Azmoon.Application.Interfaces.Facad;
using EndPoint.Site.Useful.Ultimite;
using Azmoon.Common.ResultDto;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using DotNet.RateLimiter.ActionFilters;

namespace EndPoint.Site.Controllers
    {
    public class HomeController : Controller
        {
        private readonly ILogger<HomeController> _logger;
        private static readonly NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
            {
            _logger = logger;
            _environment = environment;

            }


        public IActionResult Index()
            {
            nlog.Trace("Trace");
            return View();
            }
        public IActionResult Assessment()
            {
            nlog.Trace("Trace");
            return View();
            }

        public IActionResult Quiz()
            {
            nlog.Trace("Trace");
            return View();
            }

        public IActionResult Survay()
            {
            nlog.Trace("Trace");
            return View();
            }

        public IActionResult MyPanel()
            {

            var cliaimesValues = User.Claims.Where(p => p.Type.Contains("role")).Select(p => p.Value).ToList();
            if (cliaimesValues != null && cliaimesValues.Count < 2 && cliaimesValues.FirstOrDefault().Where(p => p.ToString() == "Regestered").Any())
                {
                return RedirectToAction("Index", "Ticket");
                }

            if (cliaimesValues != null)
                {
                return RedirectToAction("MyQuestion", "Question", new { area = "Admin" });
                }
            return RedirectToAction("Index");
            }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }


        public IActionResult GetActiveQuiz(string name)
            {

            var viewHtml = this.RenderViewAsync("_PartialStudentActiveEventsAll", "", true);
            return Json(new ResultDto<string>
                {
                Data = viewHtml,
                IsSuccess = true,
                Message = "موفق"
                });
            }
        [HttpGet]
        [RateLimit(PeriodInSec = 300, Limit = 10)]
        public IActionResult Download(string filename)
            {
            if (string.IsNullOrEmpty(filename))
                {
                return Content("Filename is not provided.");
                }
            if (filename != "001.mp4")
                {
                return Content("Filename is not provided.");
                }
            string filePath = Path.Combine(_environment.WebRootPath, "media", filename);
            if (!System.IO.File.Exists(filePath))
                {
                return Content("File not found.");
                }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", filename);
            }
        }
    }
