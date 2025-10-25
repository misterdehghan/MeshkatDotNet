using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Authorization;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Domain.Entities;
using Azmoon.Application.Service.PublicRelations.NewsPerformancesServices;
using Azmoon.Application.Service.PublicRelations.Period.Communication;
using Azmoon.Persistence.Convertors;
using EndPoint.Site.Areas.PublicRelations.Helpers.NewsPerformances;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.NewsPerformances;

namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    [Authorize]
    public class NewsPerformancesController : Controller
    {
        private readonly IGetOperatorForDropDownService _getOperatorForDropDownService;
        private readonly UserManager<User> _userManager;
        private readonly IGetOperatorNameById _getOperatorNameById;
        private readonly IAddNewsPerformancesService _addNewsPerformancesService;
        private readonly IGetListNewsPerformancesService _getListNewsPerformancesService;
        private readonly IFindActiveCommunicationPeriodService _findActiveCommunicationPeriodService;
        private readonly IGetStatusRegistrationOperatorNewsService _getStatusRegistrationOperatorService;
        private readonly IGetInsertTimeForDropDownServices _getInsertTimeForDropDownServices;
        private readonly IGetYearsForDropDownServices _getYearsForDropDownServices;
        private readonly IGetNameByIdCommunicationPeriodService _getNameByIdCommunicationPeriodService;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;
        private readonly IChangeOfStatusNewsPerformancesService _changeOfStatusNewsPerformancesService;
        private readonly IGetStatusNewsPerformances _getStatusNewsPerformances;
        private readonly IDeleteNewsPerformancesService _deleteNewsPerformancesService;
        private readonly IGetDetailNewsPerformanceService _getDetailNewsPerformanceService;
        private readonly IEditNewsPerformancesService _editNewsPerformancesService;

        public NewsPerformancesController(IGetOperatorForDropDownService getOperatorForDropDownService,
             UserManager<User> userManager,
             IGetOperatorNameById getOperatorNameById,
             IAddNewsPerformancesService addNewsPerformancesService,
             IGetListNewsPerformancesService getListNewsPerformancesService,
             IFindActiveCommunicationPeriodService findActiveCommunicationPeriodService,
             IGetStatusRegistrationOperatorNewsService getStatusRegistrationOperatorService,
             IGetInsertTimeForDropDownServices getInsertTimeForDropDownServices,
             IGetYearsForDropDownServices getYearsForDropDownServices,
             IGetNameByIdCommunicationPeriodService getNameByIdCommunicationPeriodService,
             IGetNameByNormalizedNameService getNameByNormalizedNameService,
             IChangeOfStatusNewsPerformancesService changeOfStatusNewsPerformancesService,
             IGetStatusNewsPerformances getStatusNewsPerformances,
             IDeleteNewsPerformancesService deleteNewsPerformancesService,
             IGetDetailNewsPerformanceService getDetailNewsPerformanceService,
             IEditNewsPerformancesService editNewsPerformancesService)
        {
            _getOperatorForDropDownService = getOperatorForDropDownService;
            _userManager = userManager;
            _getOperatorNameById = getOperatorNameById;
            _addNewsPerformancesService = addNewsPerformancesService;
            _getListNewsPerformancesService = getListNewsPerformancesService;
            _findActiveCommunicationPeriodService = findActiveCommunicationPeriodService;
            _getStatusRegistrationOperatorService = getStatusRegistrationOperatorService;
            _getInsertTimeForDropDownServices = getInsertTimeForDropDownServices;
            _getYearsForDropDownServices = getYearsForDropDownServices;
            _getNameByIdCommunicationPeriodService = getNameByIdCommunicationPeriodService;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
            _changeOfStatusNewsPerformancesService = changeOfStatusNewsPerformancesService;
            _getStatusNewsPerformances = getStatusNewsPerformances;
            _deleteNewsPerformancesService = deleteNewsPerformancesService;
            _getDetailNewsPerformanceService = getDetailNewsPerformanceService;
            _editNewsPerformancesService = editNewsPerformancesService;
        }

        public IActionResult Index(int page = 1, int pageSize = 10, string searchKey = "", int year = 0, int? CommunicationPeriodId = null, bool? ConfirmationStatus = null)
        {
            #region YearsForDropDown
            // ایجاد نمونه از سرویس برای دریافت سال‌ها
            var yearsResult = _getYearsForDropDownServices.Execute();
            if (yearsResult.IsSuccess)
            {
                ViewBag.Years = yearsResult.Data; // ارسال سال‌ها به ویو
            }
            else
            {
                ViewBag.Years = new List<int>(); // اگر مشکلی وجود داشت، لیست خالی
            }

            // دریافت دوره‌ها بر اساس سال انتخاب‌شده
            if (year > 0) // بررسی اینکه آیا سال انتخاب شده است یا نه
            {
                var coursesResult = _getInsertTimeForDropDownServices.Execute(year);
                if (coursesResult.IsSuccess)
                {
                    ViewBag.Courses = coursesResult.Data; // ارسال دوره‌ها به ویو (با تمام جزئیات)
                }
            }
            else
            {
                ViewBag.Courses = new List<ResultInsertTimeForDropDown>(); // اگر سال انتخاب نشده باشد، لیست دوره‌ها خالی باشد
            }
            #endregion

            // افزودن بررسی برای CommunicationPeriodId
            if (CommunicationPeriodId == null || CommunicationPeriodId == 0)
            {
                TempData["ErrorMessage"] = "لطفاً یک دوره انتخاب کنید.";
                return View(); // در صورت خالی بودن دوره، ویو بدون داده نمایش داده می‌شود
            }

            ViewBag.CurrentFilter = searchKey;
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            // اگر CommunicationPeriodId صحیح بود، به درخواست ادامه دهید
            if (CommunicationPeriodId != null)
            {
                // ایجاد درخواست صفحه‌بندی
                var request = new RequestListNewsP
                {
                    searchKey = searchKey,
                    page = page,
                    pageSize = pageSize,
                    NormalizedName = user.Operator,
                    CommunicationPeriodId = CommunicationPeriodId.Value,
                    ConfirmationStatus = ConfirmationStatus
                };


                bool isActive = _findActiveCommunicationPeriodService.Execute();
                ViewBag.IsActive = isActive; // ارسال نتیجه به ویو

                bool isOperatorMatch = user.Operator == "Senior"; // بررسی شرط
                ViewBag.IsOperatorMatch = isOperatorMatch;  // ارسال نتیجه به ویو


                // اجرای سرویس برای دریافت نتایج صفحه‌بندی‌شده
                var result = _getListNewsPerformancesService.Execute(request);

                // بررسی موفقیت‌آمیز بودن درخواست
                if (result.IsSuccess)
                {
                    // بررسی خالی بودن لیست اخبار
                    if (result.Data.newsDtos == null || !result.Data.newsDtos.Any())
                    {
                        TempData["InfoMessage"] = "در این دوره هیچ اطلاعاتی وارد نشده است."; // پیام به کاربر در صورت خالی بودن لیست
                    }

                    // انتقال اطلاعات صفحه‌بندی به ویو
                    ViewBag.PageNumber = result.Data.CurrentPage;
                    ViewBag.PageSize = result.Data.PageSize;
                    ViewBag.TotalRowCount = result.Data.RowCount;

                    // for ExportToExcel
                    ViewBag.CommunicationPeriodId = CommunicationPeriodId.Value;

                    // ارسال لیست رسانه‌ها به ویو
                    return View(result.Data);
                }

                // در صورت خطا، نمایش یک پیام خطا
                TempData["ErrorMessage"] = result.Message;
                return View("Error");
            }

            return View();

        }

        [HttpGet]
        public IActionResult GetCoursesByYear(int year)
        {

            // دریافت دوره‌ها بر اساس سال انتخاب شده
            var courses = _getInsertTimeForDropDownServices.Execute(year);
            return Json(courses.Data);  // ارسال به صورت JSON
        }

        static NewsPerformancesController()
        {
            // تنظیم مقدار LicenseContext قبل از استفاده از ExcelPackage
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // اکشن برای خروجی گرفتن به فرمت اکسل
        public IActionResult ExportToExcel(int page = 1, int pageSize = 0, string searchKey = "", int year = 0, int? CommunicationPeriodId = null, bool? ConfirmationStatus = null)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            var request = new RequestListNewsP
            {
                searchKey = searchKey,
                page = page,
                pageSize = int.MaxValue,
                NormalizedName = user.Operator,
                CommunicationPeriodId = CommunicationPeriodId.Value,
                ConfirmationStatus = ConfirmationStatus
            };

            var result = _getListNewsPerformancesService.Execute(request).Data;
            // بررسی اینکه result به عنوان لیست تعریف شده
            if (result != null && result.newsDtos != null && result.newsDtos.Any())
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("NewsPerformances");

                    // اضافه کردن سرستون‌ها
                    worksheet.Cells[1, 1].Value = "ردیف";
                    worksheet.Cells[1, 2].Value = "اوپراتور";
                    worksheet.Cells[1, 3].Value = "نام خبرگزرای";
                    worksheet.Cells[1, 4].Value = "موضوع";
                    worksheet.Cells[1, 5].Value = "تاریخ انتشار";
                    worksheet.Cells[1, 6].Value = "مستند";
                    worksheet.Cells[1, 7].Value = "وضعیت آماری";
                    // در صورت نیاز سرستون‌های بیشتری اضافه کنید

                    // اضافه کردن داده‌ها
                    for (int i = 0; i < result.newsDtos.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = i + 1;
                        worksheet.Cells[i + 2, 2].Value = result.newsDtos[i].Operator;
                        worksheet.Cells[i + 2, 3].Value = result.newsDtos[i].NewsAgencyName;
                        worksheet.Cells[i + 2, 4].Value = result.newsDtos[i].Subject;
                        worksheet.Cells[i + 2, 5].Value = result.newsDtos[i].PublicationDate.ToShamsi();

                        // اضافه کردن تصویر
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", result.newsDtos[i].Image);
                        if (System.IO.File.Exists(imagePath))
                        {
                            var image = new FileInfo(imagePath);
                            var picture = worksheet.Drawings.AddPicture($"Image{i}", image);
                            picture.SetPosition(i + 1, 0, 5, 0); // تنظیم موقعیت تصویر در اکسل (i+1 برای شماره ردیف، 5 برای ستون F)
                            picture.SetSize(100, 100); // تنظیم اندازه تصویر

                            worksheet.Row(i + 2).Height = 80; // تنظیم ارتفاع ردیف برای نمایش تصویر
                        }
                        // تنظیم عرض ستون برای نمایش تصاویر
                        worksheet.Column(6).Width = 18; // ستون F (شماره 6) برای تصاویر


                        worksheet.Cells[i + 2, 7].Value = result.newsDtos[i].Confirmation;
                        // در صورت نیاز داده‌های بیشتری اضافه کنید
                    }

                    // ذخیره کردن فایل اکسل در حافظه
                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NewsPerformances.xlsx");
                    }


                }
            }
            else
            {
                // Handling the case when result is null or does not contain data
                return BadRequest("No data available to export.");
            }
        }

        public IActionResult ExportToPdf(int page = 1, int pageSize = 0, string searchKey = "", int year = 0, int? CommunicationPeriodId = null, bool? ConfirmationStatus = null)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var request = new RequestListNewsP
            {
                searchKey = searchKey,
                page = page,
                pageSize = int.MaxValue,
                NormalizedName = user.Operator,
                CommunicationPeriodId = CommunicationPeriodId.Value,
                ConfirmationStatus = ConfirmationStatus
            };
            var result = _getListNewsPerformancesService.Execute(request).Data;

            if (result != null && result.newsDtos != null && result.newsDtos.Any())
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4, 5, 5, 100, 100);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PublicRelations", "Theme", "dist", "font", "B NAZANIN.TTF");
                    BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font footerFont = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    writer.PageEvent = new PdfPageEvents(footerFont);
                    pdfDoc.Open();

                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font cellFont = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // افزودن سربرگ اصلی به PDF
                    //Paragraph title = new Paragraph("معاونت تبلیغات و روابط عمومی سازمان عقیدتی سیاسی انتظامی ج.ا ایران", titleFont);

                    //title.Alignment = Element.ALIGN_RIGHT; // تنظیم تراز به راست
                    //pdfDoc.Add(title);
                    //pdfDoc.Add(new Paragraph("\n")); // فاصله بین سربرگ و محتوا


                    // افزودن محتوای جدول به PDF
                    PdfPTable table = new PdfPTable(6)
                    {
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL // راستچین کردن جدول
                    };


                    float[] columnWidths = { 1.8f, 1f, 2f, 1.5f, 1.5f, 0.5f };
                    table.SetWidths(columnWidths);

                    float topRowHeight = 35f;
                    float otherRowHeight = 50f;

                    // رنگ پس‌زمینه برای ردیف بالایی
                    BaseColor topRowBackgroundColor = new BaseColor(211, 211, 211);

                    // افزودن سرستونها به جدول
                    table.AddCell(new PdfPCell(new Phrase("ردیف", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("استان/ رده", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("نام خبرگزاری", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("موضوع", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("تاریخ انتشار", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("مستند", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });

                    // تکرارسر ستونها در هر صفحه
                    table.HeaderRows = 1;


                    // افزودن دادهها به جدول
                    for (int i = 0; i < result.newsDtos.Count; i++)
                    {
                        table.AddCell(new PdfPCell(new Phrase((i + 1).ToString(), cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        table.AddCell(new PdfPCell(new Phrase(result.newsDtos[i].Operator, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        table.AddCell(new PdfPCell(new Phrase(result.newsDtos[i].NewsAgencyName, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        table.AddCell(new PdfPCell(new Phrase(result.newsDtos[i].Subject, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        table.AddCell(new PdfPCell(new Phrase(result.newsDtos[i].PublicationDate.ToShamsi(), cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });

                        // افزودن تصویر به جدول
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", result.newsDtos[i].Image);
                        if (System.IO.File.Exists(imagePath))
                        {
                            var img = iTextSharp.text.Image.GetInstance(imagePath);
                            img.ScaleToFit(88f, 88f); // تنظیم اندازه تصویر

                            PdfPCell cell = new PdfPCell(img)
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                VerticalAlignment = Element.ALIGN_MIDDLE, // تنظیم عمودی تصویر در وسط
                                Padding = 5f // تنظیم فاصله داخلی برای زیبایی بیشتر

                            };

                            table.AddCell(cell);
                        }
                        else
                        {
                            table.AddCell(new PdfPCell(new Phrase("تصویر موجود نیست", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        }

                    }

                    pdfDoc.Add(table);
                    pdfDoc.Close();


                    return File(stream.ToArray(), "application/pdf", "NewsPerformances.pdf");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "هیچ دیتایی برای خروجی پیدیاف موجود نیست";
                return View();
            }
        }

        public IActionResult Add()
        {
            ViewBag.Operator = new SelectList(_getOperatorForDropDownService.Execute().Data, "Id", "Name");

            bool isActive = _findActiveCommunicationPeriodService.Execute();
            ViewBag.IsActive = isActive; // ارسال نتیجه به ویو
            return View();
        }

        [HttpPost]
        public IActionResult Add(RequestNewsPerformancesDto requestNews)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            if (!ModelState.IsValid)
            {
                // خطاهای مدل را به فرمت JSON برمی‌گردانیم
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                return Json(new { isSuccess = false, errors });
            }
            if (user != null)
            {
                if (user.Operator == "Senior" && ModelState.IsValid)
                {
                    var operatorName = _getOperatorNameById.Execute(requestNews.Operator);
                    _addNewsPerformancesService.Execute(new RequestNewsPerformances
                    {
                        NewsAgencyName = requestNews.NewsAgencyName,
                        Subject = requestNews.Subject,
                        PublicationDate = requestNews.PublicationDate,
                        Image = requestNews.Image,
                        Operator = _getNameByNormalizedNameService.Execute(operatorName).Data

                    });
                    return Json(new { isSuccess = true, message = "عملیات با موفقیت انجام شد." });
                }
                if (user.Operator != "Senior" && ModelState.IsValid)
                {
                    _addNewsPerformancesService.Execute(new RequestNewsPerformances
                    {
                        NewsAgencyName = requestNews.NewsAgencyName,
                        Subject = requestNews.Subject,
                        PublicationDate = requestNews.PublicationDate,
                        Image = requestNews.Image,
                        Operator = _getNameByNormalizedNameService.Execute(user.Operator).Data

                    });
                    return Json(new { isSuccess = true, message = "عملیات با موفقیت انجام شد." });
                }
            }
            return Json(new { isSuccess = false, message = "لطفاً تمام فیلدها را به درستی پر کنید." });
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult RegistrationStatus(int year = 0, int? CommunicationPeriodId = null)
        {
            #region YearsForDropDown

            // ایجاد نمونه از سرویس برای دریافت سال‌ها
            var yearsResult = _getYearsForDropDownServices.Execute();
            if (yearsResult.IsSuccess)
            {
                ViewBag.Years = yearsResult.Data; // ارسال سال‌ها به ویو
            }
            else
            {
                ViewBag.Years = new List<int>(); // اگر مشکلی وجود داشت، لیست خالی
            }

            // دریافت دوره‌ها بر اساس سال انتخاب‌شده
            if (year > 0) // بررسی اینکه آیا سال انتخاب شده است یا نه
            {
                var coursesResult = _getInsertTimeForDropDownServices.Execute(year);
                if (coursesResult.IsSuccess)
                {
                    ViewBag.Courses = coursesResult.Data; // ارسال دوره‌ها به ویو (با تمام جزئیات)
                }
            }
            else
            {
                ViewBag.Courses = new List<ResultInsertTimeForDropDown>(); // اگر سال انتخاب نشده باشد، لیست دوره‌ها خالی باشد
            }

            #endregion



            if (CommunicationPeriodId != null)
            {
                var result = _getStatusRegistrationOperatorService.Execute(new RequestStatusRegistration
                {
                    CommunicationPeriodId = CommunicationPeriodId.Value,
                });


                var StatisticalPeriod = _getNameByIdCommunicationPeriodService.Execute(CommunicationPeriodId.Value).Data;
                // برای نگه‌داشتن مقادیر انتخاب شده
                ViewBag.SelectedCourse = StatisticalPeriod; // آیدی دوره انتخاب‌شده
                ViewBag.SelectedYear = year; // سال انتخاب‌شده

                return View(result.Data);
            }
            else
            {
                TempData["ErrorMessage"] = "لطفاً یک دوره انتخاب کنید.";
                return View(); // در صورت خالی بودن دوره، ویو بدون داده نمایش داده می‌شود

            }

        }

        [HttpPost]
        public IActionResult ChangeOfStatus(int id, string Operator)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result.Operator;
            var result = _changeOfStatusNewsPerformancesService.Execute(id, user);

            if (result.IsSuccess)
            {
                return Json(new { success = true, message = result.Message });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
        }

        [HttpGet]
        public IActionResult GetStatus(int id)
        {
            var result = _getStatusNewsPerformances.Execute(id);
            if (result.IsSuccess)
            {
                return Json(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
        }

        public IActionResult DeleteNewsPerformances(int id)
        {
            var result = _deleteNewsPerformancesService.Execute(id);
            return Json(new
            {
                result.IsSuccess,
                result.Message
            });
        }

        public IActionResult PartialViewDetailModal(int id)
        {
            var result = _getDetailNewsPerformanceService.Execute(id);
            return PartialView(result.Data);
        }

        public IActionResult PartialViewEditModal(int id)
        {
            var result = _getDetailNewsPerformanceService.Execute(id);
            return PartialView(result.Data);
        }

        [HttpPost]
        public IActionResult PartialViewEditModal(RequestEditNewsDto request)
        {
            var result = _editNewsPerformancesService.Execute(request);
            if (result.IsSuccess)
            {
                return Json(new { success = true, message = result.Message });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
        }
    }
}

