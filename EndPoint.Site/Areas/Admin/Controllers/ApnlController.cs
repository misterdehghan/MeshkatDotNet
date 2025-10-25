using Azmoon.Application.Interfaces.Facad;
using Azmoon.Common.ResultDto;
using EndPoint.Site.Useful.Ultimite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Administrator,Teacher,Survay,Quiz,Assessment,AdminAssessment")]
    public class ApnlController : Controller
    {
        private readonly IResultQuizFacad resultQuizFacad;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ApnlController(IResultQuizFacad resultQuizFacad , IHostingEnvironment hostingEnvironment)
        {
            this.resultQuizFacad = resultQuizFacad;
            _hostingEnvironment = hostingEnvironment;
        }
       
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult test()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult tota()
        {
            return View();
        }



        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult tota(string username, long quizId, int questionCounter, int trueQuestion, int falseQuestion, DateTime StartDate, string ip)
        {
            var result = resultQuizFacad.addResultQuiz.addResultQuizAdmin(username,quizId,questionCounter,trueQuestion,falseQuestion, StartDate, ip);
            return Json(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult DeletedQuizeUser()
            {
            return View();
            }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult DeletedQuizeUser(string username, long quizId)
            {
             var result = resultQuizFacad.addResultQuiz.deletedQuizUserAdmin(username, quizId);
            return Json(result);
            }


        [HttpGet]
        public IActionResult QuizeUserStartLog()
            {
            return View();
            }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult QuizeUserStartLog(string username, long quizId)
            {
            var result = resultQuizFacad.addResultQuiz.GetQuizeUserStartLog(username, quizId);

         
            if (result.IsSuccess)
                {
                var viewHtml = this.RenderViewAsync("_PartialQuizeUserStartLog", result.Data, true);
                return Json(new ResultDto<string>
                    {
                    Data = viewHtml,
                    IsSuccess = true,
                    Message = "موفق"
                    });
                }
            return Json(new ResultDto()
                {
                IsSuccess = false,
                Message = "خطا در ارسال داده ها"
                });
         
            }
        public IActionResult Export()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("نظرسنجی");
                //First add the headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "نام";
                worksheet.Cells[1, 3].Value = "جنسیت";
                worksheet.Cells[1, 4].Value = "فروشنده ";

                //Add values
                worksheet.Cells["A2"].Value = 1000;
                worksheet.Cells["B2"].Value = "Jon";
                worksheet.Cells["C2"].Value = "M";
                worksheet.Cells["D2"].Value = 5000;

                worksheet.Cells["A3"].Value = 1001;
                worksheet.Cells["B3"].Value = "Graham";
                worksheet.Cells["C3"].Value = "M";
                worksheet.Cells["D3"].Value = 10000;

                worksheet.Cells["A4"].Value = 1002;
                worksheet.Cells["B4"].Value = "Jenny";
                worksheet.Cells["C4"].Value = "F";
                worksheet.Cells["D4"].Value = 5000;

                package.Save(); //Save the workbook.
            }
            var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.Name
            }.ToString();

            return result;
        }
    }
}
