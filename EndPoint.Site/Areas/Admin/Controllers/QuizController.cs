using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.Facad;
using Azmoon.Application.Service.Filter.Dto;
using Azmoon.Application.Service.Quiz.Dto;
using Azmoon.Common.ResultDto;
using Azmoon.Common.Useful;
using ClosedXML.Excel;
using EndPoint.Site.Useful.Ultimite;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using Stimulsoft.Base;
//using Stimulsoft.Report;
//using Stimulsoft.Report.Mvc;


namespace EndPoint.Site.Areas.Pnl.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Quiz")]
    [Authorize(Roles = "Administrator,Teacher")]
    public class QuizController : Controller
        {

        private readonly IQuizFacad _quizFacad;
        private readonly IUserFacad _userFacad;
        private readonly IDataBaseContext _context;
        private readonly IQuestionFacad _questionFacad;
        private readonly IResultQuizFacad _resultQuizFacad;
        private readonly ILogger<QuizController> _logger;
        private readonly IGroupFacad groupFacad;
        private readonly IWorkPlaceFacad _workPlaceFacad;
        private readonly IQuizFilterFacad _quizFilterFacad;
        private readonly ISession session;
        private readonly IHostingEnvironment _hostingEnvironment;
        public QuizController(IQuizFacad quizFacad, IDataBaseContext context,
            IQuestionFacad questionFacad, IResultQuizFacad resultQuizFacad,
            ILogger<QuizController> logger, IGroupFacad groupFacad, IWorkPlaceFacad workPlaceFacad,
            IUserFacad userFacad, IQuizFilterFacad quizFilterFacad, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment = null)
            {
           // //StiLicense.LoadFromString("6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2AD0gPVknKsaW0un+3PuM6TTcPMUAWEURKXNso0e5OJN40hxJjK5JbrxU+NrJ3E0OUAve6MDSIxK3504G4vSTqZezogz9ehm+xS8zUyh3tFhCWSvIoPFEEuqZTyO744uk+ezyGDj7C5jJQQjndNuSYeM+UdsAZVREEuyNFHLm7gD9OuR2dWjf8ldIO6Goh3h52+uMZxbUNal/0uomgpx5NklQZwVfjTBOg0xKBLJqZTDKbdtUrnFeTZLQXPhrQA5D+hCvqsj+DE0n6uAvCB2kNOvqlDealr9mE3y978bJuoq1l4UNE3EzDk+UqlPo8KwL1XM+o1oxqZAZWsRmNv4Rr2EXqg/RNUQId47/4JO0ymIF5V4UMeQcPXs9DicCBJO2qz1Y+MIpmMDbSETtJWksDF5ns6+B0R7BsNPX+rw8nvVtKI1OTJ2GmcYBeRkIyCB7f8VefTSOkq5ZeZkI8loPcLsR4fC4TXjJu2loGgy4avJVXk32bt4FFp9ikWocI9OQ7CakMKyAF6Zx7dJF1nZw");
            _quizFacad = quizFacad;
            _context = context;
            _questionFacad = questionFacad;
            _resultQuizFacad = resultQuizFacad;
            _logger = logger;
            this.groupFacad = groupFacad;
            _workPlaceFacad = workPlaceFacad;
            _userFacad = userFacad;
            _quizFilterFacad = quizFilterFacad;
            this.session = httpContextAccessor.HttpContext.Session;
            _hostingEnvironment = hostingEnvironment;
            }

        // GET: QuizController
        public ActionResult Index(int PageSize = 10, int PageNo = 1, string q = "", string search = "", int status = 1)
            {
            if (search == "clear")
                {
                return RedirectToAction("Index");
                }
            var result = _quizFacad.getQuiz.GetQuizes(PageSize, PageNo, q, status, User.Identity.Name);
            return View(result.Data);
            }

        // GET: QuizController/Details/5
        public ActionResult Details(long id, int PageSize = 10, int PageNo = 1)
            {
            var result = _quizFacad.getQuiz.GetQuizDetWithQuestionPagerById(id);
            var resultQus = _questionFacad.GetQuestion.GetByQuizId(PageSize, PageNo, id);
            result.Data.getQuestionWithPager = resultQus.Data;
            return View(result.Data);
            }

        // GET: QuizController/Create
        public ActionResult Create()
            {
            var categoresMentSelectListItem = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
            ViewData["categoresMentSelectListItem"] = categoresMentSelectListItem;
            return View(new AddQuizDto() { }); ;
            }

        // POST: QuizController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddQuizDto dto, IFormCollection collection)
            {

            ModelState.Remove("Id");
            if (ModelState.IsValid)
                {
                try
                    {
                    HtmlSanitizer sanitizer = new HtmlSanitizer();

                    dto.Description = sanitizer.Sanitize(dto.Description);
                    var result = _quizFacad.addQuiz.Exequte(dto, User.Identity.Name);
                    return RedirectToAction(nameof(Index));
                    }
                catch
                    {
                    var categoresMentSelectListItem = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
                    ViewData["categoresMentSelectListItem"] = categoresMentSelectListItem;
                    return View(dto);
                    }
                }
            else
                {
                var categoresMentSelectListItem = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
                ViewData["categoresMentSelectListItem"] = categoresMentSelectListItem;
                return View(dto);
                }


            }

        // GET: QuizController/Edit/5
        public ActionResult Edit(long id)
            {
            var result = _quizFacad.getQuiz.GetQuizById(id);
            var categoresMentSelectListItem = groupFacad.GetGroupSelectListItem.Exequte(null).Data;
            ViewData["categoresMentSelectListItem"] = categoresMentSelectListItem;

            return View(result.Data);
            }

        // POST: QuizController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddQuizDto dto, IFormCollection collection)
            {

            ModelState.Remove("Id");
            if (ModelState.IsValid)
                {
                try
                    {
                    HtmlSanitizer sanitizer = new HtmlSanitizer();

                    dto.Description = sanitizer.Sanitize(dto.Description);
                    var result = _quizFacad.addQuiz.Exequte(dto, User.Identity.Name);
                    return RedirectToAction("Index");
                    }
                catch
                    {
                    //var categoresMentSelectListItem = _categoreFacad.GetCategoreSelectListItem.Exequte(null).Data;
                    //TempData["categoresMentSelectListItem"] = categoresMentSelectListItem;
                    return View(dto);
                    }
                }
            else
                {
                //var categoresMentSelectListItem = _categoreFacad.GetCategoreSelectListItem.Exequte(null).Data;
                //TempData["categoresMentSelectListItem"] = categoresMentSelectListItem;
                return View(dto);
                }
            }

        // GET: QuizController/Delete/5
        public ActionResult Delete(int id)
            {
            var model = _context.Quizzes.Where(p => p.Id == id).FirstOrDefault();
            // model.Status = false;
            model.UpdatedAt = DateTime.Now;
            _context.Quizzes.Update(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            }
        public ActionResult Result(bool status, long guizId, int PageSize = 25, int PageNo = 1, string q = "", string search = "")
            {
            ViewBag.guizId = guizId;
            if (search == "clear")
                {
                return RedirectToAction("Result", new { guizId = guizId });
                }
            var model = _context.Quizzes.Where(p => p.Id == guizId).FirstOrDefault();
            if (model != null)
                {
                ViewBag.QuizName = model.Name;
                }

            var result = _resultQuizFacad.getResultQuiz.getResultWithPager(PageSize, PageNo, q, 1, guizId);
            return View(result.Data);
            }
        [HttpGet]
        public ActionResult Access(long id)
            {
            ViewData["listTypeDarajeh"] = StaticList.listTypeDarajeh;
            var model = _context.Quizzes.Where(p => p.Id == id).FirstOrDefault();
            if (model != null)
                {
                ViewBag.QuizId = model.Id;
                ViewBag.QuizName = model.Name;
                }
            var quizFilter = _quizFilterFacad.getFilter.GetByQuizId(id);
            if (quizFilter.IsSuccess)
                {
                return View(quizFilter.Data);
                }

            return View(new CreatFilterDto { Id = 0 });
            }
        [HttpGet]
        public ActionResult UserHistoryQuiz(string id, int pageIndex = 1, int pagesize = 10)
            {

            var model = _resultQuizFacad.getResultQuiz.getResultByUserId(pageIndex, pagesize, 1, id);

            return View(model.Data);


            }
        [HttpPost]
        public ActionResult Access(CreatFilterDto dto)
            {
            ViewData["listTypeDarajeh"] = StaticList.listTypeDarajeh;
            if (String.IsNullOrEmpty(dto.UserList) && dto.WorkPlaceId == null && dto.TypeDarajeh == null)
                {
                return View(dto);
                }
            var result = _quizFilterFacad.addQuizFilter.AddFilter(dto);
            if (!result.IsSuccess)
                {
                var model = _context.Quizzes.Where(p => p.Id == dto.QuizId).FirstOrDefault();
                if (model != null)
                    {
                    dto.QuizId = model.Id;
                    ViewBag.QuizId = model.Id;
                    ViewBag.QuizName = model.Name;
                    }

                return View(dto);
                }

            return RedirectToAction("Index");
            }


        public IActionResult GetWorkPlaceTreeView(string name, string family)
            {
            var model = _workPlaceFacad.GetWorkPlace.GetTreeView();
            var viewHtml = this.RenderViewAsync("_PartialWorkPlaceTreeView", model.Data, true);
            return Json(new ResultDto<string>
                {
                Data = viewHtml,
                IsSuccess = true,
                Message = "موفق"
                });
            }
        public IActionResult GetGroupTreeView(string name, string family)
            {
            var model = groupFacad.GetGroup.GetTreeView();
            var viewHtml = this.RenderViewAsync("_PartialGroupTreeView", model.Data, true);
            return Json(new ResultDto<string>
                {
                Data = viewHtml,
                IsSuccess = true,
                Message = "موفق"
                });
            }
        public IActionResult apiSelect2(string search)
            {
            var model = _userFacad.GetUsers.apiSelectUser(search);

            return Json(model);

            }
        public async Task<IActionResult> DeleteFilter(long id, long quizId)
            {
            var model = await _quizFilterFacad.deleteQuizFilter.deleteFilterAsync(quizId, id);

            return Json(model);

            }


        public IActionResult AuthorizeResultQuiz(long rezultId)
            {
            var referer = this.HttpContext.Request.Headers["Referer"].ToString();
            var result = _resultQuizFacad.autorizResultIn.autorizationResultDb(rezultId);
            return Json(result);

            }


        public IActionResult QuizResultReportPrint(long quizId)
            {

            //var key = Useful.Static.ReportConverServicese.StimulSoftKey;
            return View("QuizResultReportPrint");
            }

        public IActionResult GetReport(long quizId)
            {

            var model = _resultQuizFacad.getResultQuiz.getStimolReportQuizByQuizId(quizId, User.Identity.Name);
            StiReport report = new StiReport();
            report.Load("wwwroot/Report/ReportQuiz.mrt");
            report.RegData("QuizReport", model.Data.QuizReports);
            report["Title_Quiz"] = model.Data.Name ?? string.Empty;
            report["EndDate"] = model.Data.EndDate ?? string.Empty;
            report["StartDate"] = model.Data.StartDate ?? string.Empty;
            report["ReporNumber"] = model.Data.QuizNumber ?? string.Empty;

            return StiNetCoreViewer.GetReportResult(this, report);
            }

        public IActionResult ViewerEvent()
            {
            return StiNetCoreViewer.ViewerEventResult(this);
            }

        [HttpPost]
        public IActionResult Lottery(long id, int count, int min, int max, IFormCollection collection)
            {
            var result = _resultQuizFacad.getResultQuiz.getResultLottery(id, count, min, max, User.Identity.Name);
            if (result.IsSuccess)
                {
                var viewHtml = this.RenderViewAsync("_PartialLataryResult", result.Data, true);
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
        public FileResult QuizResultReportExel(long quizId)
            {

            var result = _resultQuizFacad.getResultQuiz.getResultWithPager(100000, 1, "", 1, quizId);
            using (var workbook = new XLWorkbook())
                {

                var worksheet = workbook.Worksheets.Add("Employees");
                worksheet.RightToLeft = true;
                // worksheet.FirstRow()
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "R";
                worksheet.Cell(currentRow, 2).Value = "محل خدمت";
                worksheet.Cell(currentRow, 3).Value = "نام و نام خانوادگی";
                worksheet.Cell(currentRow, 4).Value = "پرسنلی";
                worksheet.Cell(currentRow, 5).Value = "موبایل";
                worksheet.Cell(currentRow, 6).Value = "نمره";
                worksheet.Cell(currentRow, 7).Value = "درصد صحیح";


                foreach (var item in result.Data.ResultQuizDtos)
                    {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                    worksheet.Cell(currentRow, 2).Value = item.WorkPlaceName;
                    worksheet.Cell(currentRow, 3).Value = item.FullName;
                    worksheet.Cell(currentRow, 4).Value = item.UserName;
                    worksheet.Cell(currentRow, 5).Value = item.Phone;
                    worksheet.Cell(currentRow, 6).Value = item.Point;
                    worksheet.Cell(currentRow, 7).Value = item.Darsad;


                    }
                using (var stream = new MemoryStream())
                    {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Quiz.xlsx");
                    }
                }
            }
        public FileResult QuizResultHtmlToExel(long quizId)
            {
            var result = _resultQuizFacad.getResultQuiz.getResultWithPager(100000, 1, "", 1, quizId);

            //string sWebRootFolder = _hostingEnvironment.WebRootPath;
            //string sFileName = @$"QuizResult.xlsx";
            //string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            //FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //if (file.Exists)
            //    {
            //    file.Delete();
            //    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //    }
            //HttpResponse Response = HttpContext.Response;
            Response.Clear();
            String htm = "";
            int i;
            htm = "<table dir =\"rtl\" border=0 cellspacing=0 cellpadding=4 style='border: 1px solid black;border-collapse:collapse;mso-yfti-tbllook:1184;width:100%;font-family:Tahoma;font-size:14px;text-align:center;'>";
            //<td>قیمت  </td>  
            htm += "<tr style='background-color:#aaff22;'><td>ردیف  </td><td>محل خدمت   </td><td>نام و نام خانوادگی   </td><td>شماره پرسنلی   </td><td>تلفن  </td><td>نمره   </td><td>درصد صحیح  </td></tr>";

            int iee = 0;
            foreach (var item in result.Data.ResultQuizDtos)
                {
                if (iee % 2 == 0)
                    {
                    htm += "<tr style='background-color:#d6d9d0;'>";
                    }
                else
                    {
                    htm += "<tr >";
                    }

                htm += "<td>" + iee.ToString() + "</td>";
                htm += "<td>" + item.WorkPlaceName + "</td>";
                htm += "<td>" + item.FullName + "</td>";
                htm += "<td>" + item.UserName + "</td>";
                htm += "<td>" + item.Phone + "</td>";
                htm += "<td>" + item.Point + "</td>";
                htm += "<td>" + item.Darsad + "</td>";
                htm += "</tr>";
                iee++;
                }
            //htm += "<tr style='background-color:#aaff22;'><td>تعداد  : " + result.Data.ResultQuizDtos.Count() + "</td><td>شماره قبض  </td><td>کد کالا  </td><td>جمع کل قیمت  </td><td>" + sumPrice + "  </td><td>گروه کالا  </td><td>یگان  </td><td>واحد  </td><td>تعداد  </td><td>کاربری  </td></tr>";
            htm += "</table>";

            File(Encoding.UTF8.GetBytes(htm), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuizResult.xls");
            return File(Encoding.UTF8.GetBytes(htm), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuizResult.xls");
            }

        public ActionResult ResultXlsx(long quizId)
            {
            var model = _resultQuizFacad.getResultQuiz.getQuizRezultXLSX("", 1, quizId);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @$"NewResultQuiz.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("مسابقه و آزمون");
                // ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("RightToLeft");
                worksheet.View.RightToLeft = true;

                //First add the headers


                worksheet.Cells[@$"A1:H{model.Count()}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[@$"A1:H{model.Count()}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                worksheet.Cells[1, 1].Value = "R";
                worksheet.Cells[1, 2].Value = "محل خدمت";
                worksheet.Cells[1, 3].Value = "نام و نام خانوادگی";
                worksheet.Cells[1, 4].Value = "پرسنلی";
                worksheet.Cells[1, 5].Value = "موبایل";
                worksheet.Cells[1, 6].Value = "نمره";
                worksheet.Cells[1, 7].Value = "درصد صحیح";
                worksheet.Cells[1, 8].Value = "تاریخ آزمون ";
                worksheet.Cells[1, 9].Value = " کدملی ";
                worksheet.Cells[1, 10].Value = " شماره حساب ";
                string[] RowAndis = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL" };
                for (int i = 0; i < model.Count(); i++)
                    {

                    var listPrint = model.ElementAt(i);


                    worksheet.Cells[RowAndis[0] + "" + (i + 2)].Value = i + 1;
                    worksheet.Cells[RowAndis[1] + "" + (i + 2)].Value = listPrint.WorkPlaceName;
                    worksheet.Cells[RowAndis[2] + "" + (i + 2)].Value = listPrint.FullName;
                    worksheet.Cells[RowAndis[3] + "" + (i + 2)].Value = listPrint.UserName;
                    worksheet.Cells[RowAndis[4] + "" + (i + 2)].Value = listPrint.Phone;
                    worksheet.Cells[RowAndis[5] + "" + (i + 2)].Value = listPrint.Point;
                    worksheet.Cells[RowAndis[6] + "" + (i + 2)].Value = listPrint.Darsad;
                    worksheet.Cells[RowAndis[7] + "" + (i + 2)].Value = listPrint.QuizStart;
                    worksheet.Cells[RowAndis[8] + "" + (i + 2)].Value = listPrint.melli;
                    worksheet.Cells[RowAndis[9] + "" + (i + 2)].Value = listPrint.NumberBankAccunt;
                    if (i % 2 == 0)
                        {
                        var celllls = @$"A{i + 2}:H{i + 2}";
                        //  Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#e6daaf");
                        worksheet.Cells[celllls].Style.Font.Name = "B Titr";
                        //worksheet.Cells[celllls].Style.Fill.BackgroundColor.Rgb(210, 196, 164);
                        // worksheet.Cells[celllls].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(210, 196, 164, 0));
                        worksheet.Cells[celllls].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[celllls].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[celllls].Style.Font.Bold = true;
                        //worksheet.Cells[celllls].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightTrellis;
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
        }
    }
