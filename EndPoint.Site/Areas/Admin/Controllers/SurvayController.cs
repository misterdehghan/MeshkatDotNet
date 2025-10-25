using Azmoon.Application.Interfaces.Facad;
using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Application.Service.Survaeis.Questiones;
using Azmoon.Application.Service.Survaeis.Results;
using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Common.ResultDto;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace EndPoint.Site.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Survay,Teacher")]

    public class SurvayController : Controller
        {
        private readonly ICrudSurvayService crudSurvayService;
        private readonly ICrudQuestionSurvay crudQuestionSurvay;
        private readonly ICrudAnswerSurvay crudAnswerSurvay;
        private readonly IGroupFacad groupFacad;
        private readonly IReportChart reportChart;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SurvayController(ICrudSurvayService crudSurvayService,
            ICrudQuestionSurvay crudQuestionSurvay,
            ICrudAnswerSurvay crudAnswerSurvay, IReportChart reportChart
            , IHostingEnvironment hostingEnvironment
            , IGroupFacad groupFacad = null)
            {
            this.crudSurvayService = crudSurvayService;
            this.crudQuestionSurvay = crudQuestionSurvay;
            this.crudAnswerSurvay = crudAnswerSurvay;
            this.groupFacad = groupFacad;
            this.reportChart = reportChart;
            _hostingEnvironment = hostingEnvironment;
            }
        public ActionResult Index(int PageSize = 10, int PageNo = 1, string q = "", string search = "", int status = 1)
            {
            if (search == "clear")
                {
                return RedirectToAction("Index");
                }
            var result = crudSurvayService.GetListPageineted(PageNo, PageSize, q, status, User.Identity.Name);
            return View(result);
            }
        public IActionResult AddSurvay()
            {
            IEnumerable<SelectListItem> gl = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
            return View(new AddSurvayDto() { GroupSelectList = gl });
            }
        [HttpPost]
        public IActionResult AddSurvay(AddSurvayDto dto)
            {
            ModelState.Remove("Id");
            if (ModelState.IsValid && dto.Features.Count > 0)
                {
                //try
                //{
                HtmlSanitizer sanitizer = new HtmlSanitizer();

                dto.Description = sanitizer.Sanitize(dto.Description);
                var result = crudSurvayService.Add(dto, User.Identity.Name);
                return new JsonResult(new ResultDto { IsSuccess = result.IsSuccess });
                //}
                //catch
                //{
                //    IEnumerable<SelectListItem> gl = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
                //    dto.GroupSelectList = gl;
                //    return View(dto);
                //}
                }
            //else
            //{
            //    IEnumerable<SelectListItem> gl = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
            //    dto.GroupSelectList = gl;
            //    return View(dto);
            //}
            return new JsonResult(new ResultDto { IsSuccess = false, Message = "خطا در ارسال داده ها و اطلاعات !!!" });
            }
        [HttpGet]
        public IActionResult EditSurvay(long id)
            {
            var model = crudSurvayService.FindByIdForEdit(id);
            IEnumerable<SelectListItem> gl = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
            if (model.IsSuccess)
                {
                model.Data.GroupSelectList = gl;
                ViewData["TowDate"] = model.TwoDate;
                return View(model.Data);
                }
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpPost]
        public IActionResult EditSurvay(AddSurvayDto dto)
            {
            var model = crudSurvayService.Edit(dto, User.Identity.Name);
            if (model.IsSuccess)
                {
                //return Redirect(@$"/Admin/Survay/Details/{dto.Id}?PageSize=10&PageNo=1");
               return Redirect($"/Admin/Survay/index");
                }
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        public IActionResult ChandeStatSurvay(int id)
            {
            return View();
            }
        [HttpGet]
        public IActionResult CreateQuestionSurvay(long id)
            {
            var model = crudSurvayService.FindById(id);
            if (model.IsSuccess)
                {

                return View(new AddQuestionSurvayDto() { SurveyId = model.Data.Id, Id = 0 });
                }
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpPost]
        public IActionResult CreateQuestionSurvay(AddQuestionSurvayDto dto)
            {
            var model = crudQuestionSurvay.Add(dto);
            if (model.IsSuccess)
                {

                return RedirectToAction("Details", "Survay", new { area = "Admin", id = dto.SurveyId });
                }
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpGet]
        public IActionResult EditQuestionSurvay(long id)
            {
            var model = crudQuestionSurvay.FindById(id);
            if (model.IsSuccess)
                {

                return View(new AddQuestionSurvayDto()
                    {
                    SurveyId = model.Data.SurveyId,
                    Id = model.Data.Id,
                    QuestionType = model.Data.QuestionType,
                    Text = model.Data.Text
                    });
                }
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpGet]
        public IActionResult DetailsQuestionSurvay(long id)
            {
            var model = crudQuestionSurvay.GetQuestionDetailsWithAnswersPager(id);
            if (model.IsSuccess)
                {

                return View(model.Data);
                }
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpGet]
        public IActionResult DeleteAnswer(long id)
            {
            var model = crudAnswerSurvay.Remove(id);

            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpGet]
        public IActionResult Delete(long id)
            {
            var result = crudSurvayService.Remove(id);

            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        public IActionResult DeleteQuestionSurvay(long id)
            {
            var model = crudQuestionSurvay.Remove(id);

            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpPost]
        public IActionResult EditQuestionSurvay(AddQuestionSurvayDto dto)
            {
            var model = crudQuestionSurvay.Edit(dto);
            if (model.IsSuccess)
                {

                return RedirectToAction("Details", "Survay", new { area = "Admin", id = dto.SurveyId });
                }
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpGet]
        public IActionResult GetAnsweresQuestionSurvay(long id)
            {
            var model = crudAnswerSurvay.GetListPageineted(1, 10, id);
            List<GetAnswerSurvayViewModel> dd = new List<GetAnswerSurvayViewModel>();
            dd.Add(new GetAnswerSurvayViewModel
                {
                Id = 0,
                Index = 0,
                SurveyQuestionId = 0,
                Title = "هنوز جوابی ثبت نشده است ",
                Wight = 5
                });
            if (model.Data.Count() < 1)
                {
                return Json(dd);
                }
            return Json(model.Data);
            }
        [HttpPost]
        public IActionResult AddAnswerQuestionSurvay(AddAnswerSurvayDto dto)
            {
            var result = crudAnswerSurvay.Add(dto);
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpPost]
        public IActionResult EditAnswerQuestionSurvay(AddAnswerSurvayDto dto, long QuizId)
            {
            var result = crudAnswerSurvay.Edit(dto);
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(referer);
            }
        [HttpPost]
        public IActionResult EditAnsweSurvay(EditFeature_dto dto)
            {
            var result = crudAnswerSurvay.EditKeyAnswer(dto);
            //var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            //  return Redirect(referer);
            var model = crudAnswerSurvay.FindAnswerKeyById(dto.Id);
            return PartialView("_PartialEditKeyAnswer", model.Data);
            }
        public ActionResult Details(long id, int PageSize = 20, int PageNo = 1)
            {
            var model = crudQuestionSurvay.GetListPageineted(PageNo, PageSize, id);
            return View(model);
            }
        public ActionResult Result(string q, bool status, long survayId, int PageSize = 20, int PageNo = 1)
            {
            var model = reportChart.Latary(survayId);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @$"{model.Data.survayTitle.Replace(" ", "_")}.xlsx";
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
                var counterHeader = 1;
                foreach (var item in model.Data.survayQuestionTitle)
                    {
                    worksheet.Cells[1, counterHeader].Value = item;
                    counterHeader++;
                    }
                string[] RowAndis = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL" };
                for (int i = 0; i < model.Data.GetAnswers.Count; i++)
                    {

                    var listPrint = model.Data.GetAnswers.ElementAt(i);
                    for (int j = 0; j < listPrint.Count(); j++)
                        {
                        var lableCell = RowAndis[j] + "" + (i + 2);
                        worksheet.Cells[lableCell].Value = listPrint.ElementAt(j);
                        }

                    }


                package.Save(); //Save the workbook.
                }
            var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
                {
                FileName = file.Name
                }.ToString();
            return result;
            }

        public ActionResult ResultDescription(long survayId)
            {
            var model = reportChart.LataryDescriptions(survayId);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @$"{model.Data.survayTitle}.xlsx";
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
                worksheet.Cells[1, 1].Value = "survayId";
                worksheet.Cells[1, 2].Value = "QuestionId";
                worksheet.Cells[1, 3].Value = "QuestionTitle";
                worksheet.Cells[1, 4].Value = "Ip";
                worksheet.Cells[1, 5].Value = "WorkPlaceUser";
                worksheet.Cells[1, 6].Value = "text";
       
                string[] RowAndis = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL" };
                for (int i = 0; i < model.Data.GetAnseres.Count; i++)
                    {

                    worksheet.Cells[i+2 , 1].Value = model.Data.survayId;
                    worksheet.Cells[i+2 , 2].Value = model.Data.GetAnseres.ElementAt(i).QuestionId;
                    worksheet.Cells[i+2 , 3].Value = model.Data.GetAnseres.ElementAt(i).QuestionTitle;
                    worksheet.Cells[i+2 , 4].Value = model.Data.GetAnseres.ElementAt(i).Ip;
                    worksheet.Cells[i+2 , 5].Value = model.Data.GetAnseres.ElementAt(i).WorkPlaceUser;
                    worksheet.Cells[i+2 , 6].Value = model.Data.GetAnseres.ElementAt(i).Text;

                    }


                package.Save(); //Save the workbook.
                }
            var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
                {
                FileName = file.Name
                }.ToString();
            return result;
            }
        public FileResult ExportCSV(long survayId)
            {

            var model = reportChart.Latary(survayId);
            StringBuilder sb = new StringBuilder();

            foreach (var item in model.Data.survayQuestionTitle)
                {

                sb.Append(item + ',');

                }
            sb.Append("\r\n");
            for (int i = 0; i < model.Data.GetAnswers.Count; i++)
                {

                var listPrint = model.Data.GetAnswers.ElementAt(i);
                for (int j = 0; j < listPrint.Count(); j++)
                    {
                    sb.Append(listPrint.ElementAt(j) + ',');


                    }
                sb.Append("\r\n");
                }


            return File(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "نظرسنجی.csv");
            }
        }
    }
