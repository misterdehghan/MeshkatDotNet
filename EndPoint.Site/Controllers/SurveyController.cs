using Azmoon.Application.Service.Survaeis.Results;
using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities;
using DotNet.RateLimiter.ActionFilters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class SurveyController : Controller
    {

        private readonly SignInManager<User> _signInManager;
        private readonly IGetStartSurvay getStartSurvay;
        private readonly IAddResultSurvay _addResultSurvay;
        private string userId = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IReportChart reportChart;

        public SurveyController(IGetStartSurvay getStartSurvay,
            SignInManager<User> signInManager,
            IAddResultSurvay addResultSurvay,
            IHostingEnvironment hostingEnvironment,
            IReportChart reportChart)
            {
            this.getStartSurvay = getStartSurvay;
            _signInManager = signInManager;
            _addResultSurvay = addResultSurvay;
            _hostingEnvironment = hostingEnvironment;
            this.reportChart = reportChart;
            }


        [HttpPost]
        public IActionResult Start( long survayId)
        {
            var model = getStartSurvay.start(survayId);
            if (!model.IsSuccess && model==null && model.Data==null&&model.Data.getQuestion.Count()<1)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model.Data);
        }
        [HttpPost]
        [RateLimit(PeriodInSec = 30, Limit = 4)]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(IFormCollection formCollection , long WorkPlaceId)
        {
            if (!ModelState.IsValid)
            {

                return RedirectToAction("AccessDenied", "Account", new { area = "", message = "شما بصورت غیر مجاز قصد شرکت  در نظرسنجی را داشته اید!!!!" });

            }
            var ip = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");
            if (String.IsNullOrEmpty(ip))
                {
                IPAddress ipAddress;
                var headers = Request.Headers.ToList();
                if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
                    {
                    // when running behind a load balancer you can expect this header
                    var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                    // in case the IP contains a port, remove ':' and everything after
                    ipAddress = IPAddress.Parse(header.Remove(header.IndexOf(':')));
                    }
                else
                    {
                    // this will always have a value (running locally in development won't have the header)
                    ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
                    }
                ip = ipAddress.ToString();
                }

            List<KeyValuePair<string, string>> answer = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> deccriptionAnswer = new List<KeyValuePair<string, string>>();
            var id = formCollection.Where(p => p.Key == "survayId").FirstOrDefault().Value;
            foreach (var item in formCollection)
            {

            

                if (item.Key.StartsWith("jameiatanswer") ||  item.Key.StartsWith("chandanswer"))
                {
                    var Key = (item.Key).Split('_');
                    answer.Add(new KeyValuePair<string, string>(Key[1], item.Value));

                }
           
                if (item.Key.StartsWith("deccriptionAnswer"))
                {
                    var Key = (item.Key).Split('_');
                    deccriptionAnswer.Add(new KeyValuePair<string, string>(Key[1], item.Value));

                }


            }
            if (answer.Count()>1)
            {
                DataResultSurvayDto model = new DataResultSurvayDto();
                model.answer = answer;
                model.deccriptionAnswers = deccriptionAnswer;
                model.SurvayId = Int64.Parse(id);
                model.Ip = ip;   
                model.WorkPlaceId = WorkPlaceId;
                model.username = GetOrSetBasket();
                var result = _addResultSurvay.addResultSurvay(model);

                return Json(result);
            }
            return Json(new ResultDto() {IsSuccess=false,Message="خطا در ارسال اطلاعات توسط کاربر!!!!" });

        }
        [HttpGet]
        public IActionResult Start()
        {

            return RedirectToAction("Error" , "Home");
        }
        public ActionResult ReportWorckplaceResult(string uniqKay)
            {
            var model = reportChart.ReportSurvayWorckPlaceDto(uniqKay);
            ViewBag.uniqKay = uniqKay;
            return View(model.Data);
            }
        public ActionResult ReportWorckplaceEXELresult(string uniqKay)
            {
            var model = reportChart.ReportSurvayWorckPlaceDto(uniqKay);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @$"آمار_مشارکت_{model.Message.Replace(" ", "_")}.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sFileName);
                //First add the headers
                var counterHeader = 1;
                worksheet.View.RightToLeft = true;

                //First add the headers


                worksheet.Cells[@$"A1:C{model.Data.Count}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[@$"A1:C{model.Data.Count}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells[@$"A1:C1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[@$"A1:C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                worksheet.Cells[@$"A1:C1"].Style.Font.Bold = true;
                worksheet.Cells[@$"A1:C1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[@$"A1:C1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[@$"A1:C1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[@$"A1:C1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                string[] RowAndis = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL" };

                worksheet.Cells[1, 1].Value = "R";
                worksheet.Cells[1, 2].Value = "عنوان رده";
                worksheet.Cells[1, 3].Value = "مشارکت(نفر)";
         

                for (int i = 0; i < model.Data.Count; i++)
                    {

                    worksheet.Cells[i + 2, 1].Value = counterHeader;
                    worksheet.Cells[i + 2, 2].Value = model.Data.ElementAt(i).WorckPlaceTitle;
                    worksheet.Cells[i + 2, 3].Value = model.Data.ElementAt(i).Nafarat;
                   
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
                    counterHeader = counterHeader+1;

                    }
                worksheet.Cells[model.Data.Count+2, 1].Value = counterHeader;
                worksheet.Cells[model.Data.Count+2, 2].Value ="جمع کل";
                worksheet.Cells[model.Data.Count+2, 3].Value = model.Data.Sum(p=>p.Nafarat);
                package.Save(); //Save the workbook.
                }
            var result = PhysicalFile(Path.Combine(sWebRootFolder, sFileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
                {
                FileName = file.Name
                }.ToString();
            return result;
            }
        private string GetOrSetBasket()
        {

            if (_signInManager.IsSignedIn(User))
            {
              var  usermyId = User.Identity.Name;
                userId = usermyId;
                return userId;
            }
            else
            {
                return SetCookiesForSurvay();
               
            }
        }
        private string SetCookiesForSurvay()
        {
            string survayCookieName = "VisitorId";
            if (Request.Cookies.ContainsKey(survayCookieName))
            {
                userId = Request.Cookies[survayCookieName];
            }
            if (userId != null) return userId;
            userId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true };
            cookieOptions.Expires = DateTime.Today.AddYears(2);
            Response.Cookies.Append(survayCookieName, userId, cookieOptions);

            return userId;
        }

    }
}
