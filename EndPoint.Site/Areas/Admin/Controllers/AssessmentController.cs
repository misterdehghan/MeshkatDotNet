using Azmoon.Application.Interfaces.Assessment;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Assessment;
using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Application.Service.Survaeis.Results;
using Azmoon.Application.Service.UserAccess.Query;
using Azmoon.Common.ResultDto;
using EndPoint.Site.Useful.Ultimite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Stimulsoft.Data.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Assessment,AdminAssessment")]
    public class AssessmentController : Controller
        {
        private readonly IWriteAssessment _writeAssessment;
        private readonly IGetAssessment _getAssessment;
        private readonly IUserAccessService _userAccess;
        private readonly IDataBaseContext _context;    
        private readonly IGetResultAssesment _resultAssesment;
        private readonly IGetAssessmentUserAccessWorkPalce _assessmentUserAccessWorkPalce;
        private readonly IHostingEnvironment _hostingEnvironment;
        public AssessmentController(IWriteAssessment writeAssessment,
            IGetAssessment getAssessment, IUserAccessService userAccess,
            IDataBaseContext context,
            IGetAssessmentUserAccessWorkPalce assessmentUserAccessWorkPalce,
            IGetResultAssesment resultAssesment, IHostingEnvironment hostingEnvironment)
            {
            this._writeAssessment = writeAssessment;
            _getAssessment = getAssessment;
            _userAccess = userAccess;
            _context = context;
            _assessmentUserAccessWorkPalce = assessmentUserAccessWorkPalce;
            _resultAssesment = resultAssesment;
            _hostingEnvironment = hostingEnvironment;
            }

        public IActionResult Index()
            {
            var userIsAdmin = User.IsInRole("AdminAssessment");
            var model = _getAssessment.GetAssessmentPagination(User.Identity.Name, 1, 10 , userIsAdmin);
            return View(model.Data);
            }
        public IActionResult AddAssessment()
            {
            var model = new AddAssessmentDto() { };
            model.TemplateSelectList = _context.TemplateMains.Where(p=>p.Status!=3).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            return View(model);
            } 
        public IActionResult EditAssessment(int id)
            {
            var model = _writeAssessment.GetAssessment(id, User.Identity.Name).Data;
            model.TemplateSelectList = _context.TemplateMains.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            return View(model);
            }
        [HttpPost]
        public IActionResult AddAssessment(AddAssessmentDto dto)
            {
            var model = _writeAssessment.AddAssessment(dto, User.Identity.Name);
            if (model.IsSuccess)
            {
                return new JsonResult(new ResultDto { IsSuccess = true, Message = "ثبت موفق " });
                }
            return new JsonResult(new ResultDto { IsSuccess = false, Message = "ثبت ناموفق " });
            }
        public IActionResult GetDiteles(int id)
            {
            var model = _getAssessment.GetTemplateQustionAnswers(id, User.Identity.Name).Data;
            return View(model);
            } 
        public IActionResult viewTemplateDiteles(int id)
            {
            var model = _getAssessment.GetViewTemplateQustionAnswers(id, User.Identity.Name).Data;
            return View("GetDiteles", model);
            }
        [Authorize(Roles = "Administrator,AdminAssessment")]
        public IActionResult EditTemplateQustionAnswer(int id)
            {
            var model = _getAssessment.GetTemplateQustionAnswers(id, User.Identity.Name).Data;
            return View(model);
            }
        [Authorize(Roles = "Administrator,AdminAssessment")]
        [HttpPost]
        public IActionResult EditTemplateQustionAnswer(IFormCollection foFormCollection ,int TemplateId ,string TemplateName)
            {
            List<KeyValuePair<string, string>> answerFeaturTitle = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> answerFeaturWight = new List<KeyValuePair<string, string>>(); 
            List<KeyValuePair<string, string>> QuestionFeaturTitle = new List<KeyValuePair<string, string>>();
            foreach (var item in foFormCollection)
                {

                if (item.Key.StartsWith("answerFeaturTitle"))
                    {
                    var Key = (item.Key).Split('_');
                    answerFeaturTitle.Add(new KeyValuePair<string, string>(Key[1], item.Value));

                    }


                }
            foreach (var item in foFormCollection)
                {

                if (item.Key.StartsWith("answerFeaturWight"))
                    {
                    var Key = (item.Key).Split('_');
                    answerFeaturWight.Add(new KeyValuePair<string, string>(Key[1], item.Value));

                    }


                } 
            foreach (var item in foFormCollection)
                {

                if (item.Key.StartsWith("QuestionFeaturTitle"))
                    {
                    var Key = (item.Key).Split('_');
                    QuestionFeaturTitle.Add(new KeyValuePair<string, string>(Key[1], item.Value));

                    }


                }

            var result = _getAssessment.EditTemplateQustionAnswers(new EditTemplateQustionAnswersDto() {
                answerFeaturTitle = answerFeaturTitle,  
                answerFeaturWight = answerFeaturWight,
                QuestionFeaturTitle = QuestionFeaturTitle,
                TemplateId = TemplateId
                }, User.Identity.Name , TemplateName);
            return RedirectToAction("Templates");
            }
   
        public IActionResult Templates()
            {
            var modle = _getAssessment.GetTemplates(User.Identity.Name).Data;
            return View(modle);
            }
        [Authorize(Roles = "Administrator,AdminAssessment")]
        [HttpGet]
        public IActionResult CreateTemplate()
            {
            return View();
            }
        [Authorize(Roles = "Administrator,AdminAssessment")]
        [HttpPost]
        public IActionResult CreateTemplate(AddTemplateDto dto)
            {
            if (dto != null)
                {
                if (dto.AnswerFeatures.Count > 1 && dto.QuestionFeatures.Count > 0)
                    {
                    var result = _writeAssessment.Add(dto, User.Identity.Name);
                    return new JsonResult(new ResultDto { IsSuccess = result.IsSuccess, Message = result.Message });
                    }

                return new JsonResult(new ResultDto { IsSuccess = false, Message = "تعداد سوال و جواب ها کمتراز حد نرمال می باشد !!!" });
                }

            return new JsonResult(new ResultDto { IsSuccess = false, Message = "خطا در ارسال اطلاعات !!!" });

            }
        [HttpPost]
        public IActionResult GetAccess(string username)
            {
            var model = _userAccess.Getes(username).Data;
            return PartialView("_PartialViewGetAccess", model);
            }
        [HttpPost]
        public IActionResult AddAccessToUser(string username, int wp)
            {
            if (!string.IsNullOrEmpty(username))
                {

                var result = _userAccess.Add(wp, username, User.Identity.Name).Data;
                }
            var model = _userAccess.Getes(username).Data;
            return PartialView("_PartialViewGetAccess", model);
            }

        [HttpPost]
        public IActionResult DeletedAccess(int id)
            {
            var result = _userAccess.DeleteAccess(id);
            if (result.IsSuccess)
                {
                var model = _userAccess.Getes(result.Data).Data;
                return PartialView("_PartialViewGetAccess", model);
                }
 
            return PartialView("_PartialViewGetAccess", null);
            }
        [HttpGet]
        public async Task<IActionResult> WPartial() {
            var model = _assessmentUserAccessWorkPalce.GetTreeViewDto(User.Identity.Name);
            return PartialView("GetUserWorkPlaceTreeView", model);
            }
        public ActionResult Result(int assessmentId)
            {
            var model = _resultAssesment.Latary(assessmentId);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @$"{model.Data.survayTitle.Replace(" ","_")}.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ارزشیابی_دوره");
                //First add the headers
                worksheet.Cells[1, 1].Value = "R";
                var counterHeader = 2;
                foreach (var item in model.Data.survayQuestionTitle)
                    {
                    worksheet.Cells[1, counterHeader].Value = item;
                    counterHeader++;
                    }
                string[] RowAndis = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL" };
                for (int i = 0; i < model.Data.GetAnswers.Count; i++)
                    {
                    var FCell = "A" + (i+2);
                     worksheet.Cells[FCell].Value = (i + 1).ToString();
                   // worksheet.Cells[1, (i + 2)].Value = (i + 1).ToString();
                    var listPrint = model.Data.GetAnswers.ElementAt(i);
                    for (int j = 0; j < listPrint.Count(); j++)
                        {
                        var lableCell = RowAndis[j + 1] + "" + (i + 2);
                        
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

        public ActionResult ResultDescription(int assessmentId)
            {
            var model = _resultAssesment.AssessmentDescriptions(assessmentId);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @$"{model.Data.AssessmentTitle.Replace(" ","_")}_تشزیحی.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ارزشیابی_دوره_تشریحی");
                //First add the headers
                worksheet.Cells[1, 1].Value = "R";
                worksheet.Cells[1, 2].Value = "AssessmentId";
                worksheet.Cells[1, 3].Value = "DateTime";
                worksheet.Cells[1, 4].Value = "Ip";
                worksheet.Cells[1, 5].Value = "WorkPlaceUser";
                worksheet.Cells[1, 6].Value = "text";

                string[] RowAndis = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL" };
                for (int i = 0; i < model.Data.GetAnseres.Count; i++)
                    {
                    var ACell = "A" + (i + 2);
                    worksheet.Cells[ACell].Value = (i + 1).ToString();
                    var BCell = "B" + (i + 2);
                    worksheet.Cells[BCell].Value = model.Data.AssessmentId;
                    worksheet.Cells[i + 2, 3].Value = model.Data.GetAnseres.ElementAt(i).datetime;
                    worksheet.Cells[i + 2, 4].Value = model.Data.GetAnseres.ElementAt(i).Ip;
                    worksheet.Cells[i + 2, 5].Value = model.Data.GetAnseres.ElementAt(i).WorkPlaceUser;
                    worksheet.Cells[i + 2, 6].Value = model.Data.GetAnseres.ElementAt(i).Text;

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

        [HttpGet]
        public IActionResult GetSearchAssessment(string qSearch , DateTime? StartDate , DateTime? EndDate , long WorkPlaceId=0 , string WorkPlaceIdFake="")
            {
            var model = _getAssessment.GetSearchAssessment(User.Identity.Name, qSearch,StartDate, EndDate , WorkPlaceId);
           // var result =this. RenderRazorViewToString("_PartialViewGetSearchAssessment", model.Data);

           //return new JsonResult(new ResultDto<string> { IsSuccess = true, Message = "ثبت موفق " ,Data=result});
            return PartialView("_PartialViewGetSearchAssessment", model.Data);
            }

        [Authorize(Roles = "Administrator,AdminAssessment")]
        [HttpGet]
        [Route("Admin/Assessment/deleteAssessment/{id}")]
        public IActionResult deleteAssessment(int id)
            {
            var model = _writeAssessment.DeleteAssessment(id);


            return RedirectToAction("Index");
      
            }

        }
    }

