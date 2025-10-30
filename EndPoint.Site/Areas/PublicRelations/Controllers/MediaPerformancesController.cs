using Application.Services.Period.Communication;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Azmoon.Application.Service.PublicRelations.MediaPerformances;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Domain.Entities;
using Azmoon.Application.Service.PublicRelations.Period.Communication;
using Azmoon.Persistence.Convertors;
using EndPoint.Site.Areas.PublicRelations.Helpers.NewsPerformances;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.MediaPerformances;


namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    [Authorize]
    public class MediaPerformancesController : Controller
    {
        private readonly IAddMediaPerformancesService _addMediaPerformancesService;
        private readonly IGetListMediaPerformancesService _getListMediaPerformancesService;
        private readonly IGetOperatorForDropDownService _getOperatorForDropDownService;
        private readonly UserManager<User> _userManager;
        private readonly IGetOperatorNameById _getOperatorNameById;
        private readonly IFindActiveCommunicationPeriodService _findActiveCommunicationPeriodService;
        private readonly IGetStatusRegistrationOperatorMediaService _getStatusRegistrationOperatorMediaService;
        private readonly IGetYearsForDropDownServices _getYearsForDropDownServices;
        private readonly IGetInsertTimeForDropDownServices _getInsertTimeForDropDownServices;
        private readonly IGetNameByIdCommunicationPeriodService _getNameByIdCommunicationPeriodService;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;
        private readonly IGetDataForViewBagMediaPerformancesService _getDataForViewBagMediaPerformancesService;
        private readonly IChangeOfStatusMediaPerformancesService _changeOfStatusMediaPerformancesService;
        private readonly IGetStatusMediaPerformances _getStatusMediaPerformances;
        private readonly IDeleteMediaPerformancesService _deleteMediaPerformancesService;
        private readonly IGetDetailMediaPerformanceService _getDetailMediaPerformanceService;
        private readonly IEditMediaPerformancesService _editMediaPerformancesService;
        private readonly IGetSubjectForDropDownService _getSubjectForDropDown;

        public MediaPerformancesController(IAddMediaPerformancesService addMediaPerformancesService,
            IGetListMediaPerformancesService getListMediaPerformancesService,
            IGetOperatorForDropDownService getOperatorForDropDownService,
            UserManager<User> userManager,
            IGetOperatorNameById getOperatorNameById,
            IFindActiveCommunicationPeriodService findActiveCommunicationPeriodService,
            IGetStatusRegistrationOperatorMediaService getStatusRegistrationOperatorMediaService,
            IGetYearsForDropDownServices getYearsForDropDownServices,
            IGetInsertTimeForDropDownServices getInsertTimeForDropDownServices,
            IGetNameByIdCommunicationPeriodService getNameByIdCommunicationPeriodService,
            IGetNameByNormalizedNameService getNameByNormalizedNameService,
            IGetDataForViewBagMediaPerformancesService getDataForViewBagMediaPerformancesService,
            IChangeOfStatusMediaPerformancesService changeOfStatusMediaPerformancesService,
            IGetStatusMediaPerformances getStatusMediaPerformances,
            IDeleteMediaPerformancesService deleteMediaPerformancesService,
            IGetDetailMediaPerformanceService getDetailMediaPerformanceService,
            IEditMediaPerformancesService editMediaPerformancesService,
            IGetSubjectForDropDownService getSubjectForDropDown)
        {
            _addMediaPerformancesService = addMediaPerformancesService;
            _getListMediaPerformancesService = getListMediaPerformancesService;
            _getOperatorForDropDownService = getOperatorForDropDownService;
            _userManager = userManager;
            _getOperatorNameById = getOperatorNameById;
            _findActiveCommunicationPeriodService = findActiveCommunicationPeriodService;
            _getStatusRegistrationOperatorMediaService = getStatusRegistrationOperatorMediaService;
            _getYearsForDropDownServices = getYearsForDropDownServices;
            _getInsertTimeForDropDownServices = getInsertTimeForDropDownServices;
            _getNameByIdCommunicationPeriodService = getNameByIdCommunicationPeriodService;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
            _getDataForViewBagMediaPerformancesService = getDataForViewBagMediaPerformancesService;
            _changeOfStatusMediaPerformancesService = changeOfStatusMediaPerformancesService;
            _getStatusMediaPerformances = getStatusMediaPerformances;
            _deleteMediaPerformancesService = deleteMediaPerformancesService;
            _getDetailMediaPerformanceService = getDetailMediaPerformanceService;
            _editMediaPerformancesService = editMediaPerformancesService;
            _getSubjectForDropDown = getSubjectForDropDown;
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

            bool isActive = _findActiveCommunicationPeriodService.Execute();
            ViewBag.IsActive = isActive; // ارسال نتیجه به ویو

            bool isOperatorMatch = user.Operator == "Senior"; // بررسی شرط
            ViewBag.IsOperatorMatch = isOperatorMatch;  // ارسال نتیجه به ویو


            #region DataForViewBag
            var requestTotal = new RequestDataForViewBagMedia
            {
                CommunicationPeriodId = CommunicationPeriodId.Value,
                NormalizedName = user.Operator
            };
            var dataForViewBag = _getDataForViewBagMediaPerformancesService.Execute(requestTotal).Data;
            ViewBag.TotalPlayingTime = dataForViewBag.TotalPlayingTime;
            ViewBag.NumberOfRecords = dataForViewBag.NumberOfRecords;
            ViewBag.Confirmed = dataForViewBag.Confirmed;
            ViewBag.Television = dataForViewBag.Television;
            ViewBag.Radio = dataForViewBag.Radio;
            #endregion



            // اگر CommunicationPeriodId صحیح بود، به درخواست ادامه دهید
            if (CommunicationPeriodId != null)
            {

                // ایجاد درخواست صفحه‌بندی
                var request = new RequestListMPDto
                {
                    searchKey = searchKey,
                    page = page,
                    pageSize = pageSize,
                    NormalizedName = user.Operator,
                    CommunicationPeriodId = CommunicationPeriodId.Value,
                    ConfirmationStatus = ConfirmationStatus
                };

                // اجرای سرویس برای دریافت نتایج صفحه‌بندی‌شده
                var result = _getListMediaPerformancesService.Execute(request);

                // بررسی موفقیت آمیز بودن درخواست
                if (result.IsSuccess)
                {
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

        static MediaPerformancesController()
        {
            // تنظیم مقدار LicenseContext قبل از استفاده از ExcelPackage
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // اکشن برای خروجی گرفتن به فرمت اکسل
        public IActionResult ExportToExcel(int page = 1, int pageSize = 0, string searchKey = "", int year = 0, int? CommunicationPeriodId = null, bool? ConfirmationStatus = null)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            var request = new RequestListMPDto
            {
                searchKey = searchKey,
                page = page,
                pageSize = int.MaxValue,
                NormalizedName = user.Operator,
                CommunicationPeriodId = CommunicationPeriodId.Value,
                ConfirmationStatus = ConfirmationStatus
            };

            var result = _getListMediaPerformancesService.Execute(request).Data;
            // بررسی اینکه result به عنوان لیست تعریف شده
            if (result != null && result.mediaDto != null && result.mediaDto.Any())
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("MediaPerformances");

                    // اضافه کردن سرستون‌ها
                    worksheet.Cells[1, 1].Value = "ردیف";
                    worksheet.Cells[1, 2].Value = "استان/ رده";
                    worksheet.Cells[1, 3].Value = "نام رسانه";
                    worksheet.Cells[1, 4].Value = "نام شبکه";
                    worksheet.Cells[1, 5].Value = "نام برنامه";
                    worksheet.Cells[1, 6].Value = "موضوع";
                    worksheet.Cells[1, 7].Value = "تاریخ";
                    worksheet.Cells[1, 8].Value = "مدت زمان پخش";
                    worksheet.Cells[1, 9].Value = "مستند";
                    worksheet.Cells[1, 10].Value = "وضعیت آماری";
                    // در صورت نیاز سرستون‌های بیشتری اضافه کنید

                    // اضافه کردن داده‌ها
                    for (int i = 0; i < result.mediaDto.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = i + 1;
                        worksheet.Cells[i + 2, 2].Value = result.mediaDto[i].Operator;
                        worksheet.Cells[i + 2, 3].Value = result.mediaDto[i].Media;
                        worksheet.Cells[i + 2, 4].Value = result.mediaDto[i].NetworkName;
                        worksheet.Cells[i + 2, 5].Value = result.mediaDto[i].ProgramName;
                        worksheet.Cells[i + 2, 6].Value = result.mediaDto[i].SubjectTitle;
                        worksheet.Cells[i + 2, 7].Value = result.mediaDto[i].BroadcastDate.ToShamsi();
                        worksheet.Cells[i + 2, 8].Value = result.mediaDto[i].Time;

                        // اضافه کردن تصویر
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", result.mediaDto[i].Image);
                        if (System.IO.File.Exists(imagePath))
                        {
                            var image = new FileInfo(imagePath);
                            var picture = worksheet.Drawings.AddPicture($"Image{i}", image);
                            picture.SetPosition(i + 1, 0, 8, 0);
                            picture.SetSize(100, 100); // تنظیم اندازه تصویر

                            worksheet.Row(i + 2).Height = 80; // تنظیم ارتفاع ردیف برای نمایش تصویر
                        }
                        // تنظیم عرض ستون برای نمایش تصاویر
                        worksheet.Column(9).Width = 18;


                        worksheet.Cells[i + 2, 10].Value = result.mediaDto[i].Confirmation;
                        // در صورت نیاز داده‌های بیشتری اضافه کنید
                    }

                    // ذخیره کردن فایل اکسل در حافظه
                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MediaPerformances.xlsx");
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
            var request = new RequestListMPDto
            {
                searchKey = searchKey,
                page = page,
                pageSize = int.MaxValue,
                NormalizedName = user.Operator,
                CommunicationPeriodId = CommunicationPeriodId.Value,
                ConfirmationStatus = ConfirmationStatus
            };
            var result = _getListMediaPerformancesService.Execute(request).Data;

            if (result != null && result.mediaDto != null && result.mediaDto.Any())
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


                    //float[] columnWidths = { 1.8f, 1f, 2f, 1.5f, 1.5f, 0.5f };

                    float[] columnWidths = { 1.8f, 1f, 0.9f, 2.4f, 1.3f, 0.5f };
                    table.SetWidths(columnWidths);

                    float topRowHeight = 35f;
                    float otherRowHeight = 50f;

                    // رنگ پس‌زمینه برای ردیف بالایی
                    BaseColor topRowBackgroundColor = new BaseColor(211, 211, 211);

                    // افزودن سرستونها به جدول
                    table.AddCell(new PdfPCell(new Phrase("ردیف", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("استان/ رده", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });

                    string combinedTitel = $"نام رسانه / نام شبکه / نام برنامه / موضوع";
                    table.AddCell(new PdfPCell(new Phrase(combinedTitel, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });


                    //table.AddCell(new PdfPCell(new Phrase("نام رسانه", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    //table.AddCell(new PdfPCell(new Phrase("نام شبکه", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    //table.AddCell(new PdfPCell(new Phrase("نام برنامه", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    //table.AddCell(new PdfPCell(new Phrase("موضوع", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("تاریخ", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });
                    table.AddCell(new PdfPCell(new Phrase("مدت زمان پخش", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });

                    table.AddCell(new PdfPCell(new Phrase("مستند", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = topRowHeight, BackgroundColor = topRowBackgroundColor });

                    // تکرارسر ستونها در هر صفحه
                    table.HeaderRows = 1;


                    // افزودن دادهها به جدول
                    for (int i = 0; i < result.mediaDto.Count; i++)
                    {
                        table.AddCell(new PdfPCell(new Phrase((i + 1).ToString(), cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        table.AddCell(new PdfPCell(new Phrase(result.mediaDto[i].Operator, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });

                        // ترکیب چند مقدار با یک اسلش
                        string combinedText = $"{result.mediaDto[i].Media} / {result.mediaDto[i].NetworkName} / {result.mediaDto[i].ProgramName}/ {result.mediaDto[i].SubjectTitle}";
                        table.AddCell(new PdfPCell(new Phrase(combinedText, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });


                        //table.AddCell(new PdfPCell(new Phrase(result.mediaDto[i].Media, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        //table.AddCell(new PdfPCell(new Phrase(result.mediaDto[i].NetworkName, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        //table.AddCell(new PdfPCell(new Phrase(result.mediaDto[i].ProgramName, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        //table.AddCell(new PdfPCell(new Phrase(result.mediaDto[i].Subject, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        table.AddCell(new PdfPCell(new Phrase(result.mediaDto[i].BroadcastDate.ToShamsi(), cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });
                        table.AddCell(new PdfPCell(new Phrase(result.mediaDto[i].Time.ToString(), cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = otherRowHeight });


                        // افزودن تصویر به جدول
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", result.mediaDto[i].Image);
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


                    return File(stream.ToArray(), "application/pdf", "MediaPerformances.pdf");
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
            List<SelectListItem> Media = new List<SelectListItem>();
            Media.Add(new SelectListItem() { Text = "تلوزیون", Value = "1" });
            Media.Add(new SelectListItem() { Text = "رادیو", Value = "2" });

            ViewBag.Media = new SelectList(Media, "Value", "Text");

            ViewBag.Operator = new SelectList(_getOperatorForDropDownService.Execute().Data, "Id", "Name");
            ViewBag.Subjct = new SelectList(_getSubjectForDropDown.Execute().Data, "Id", "Name");


            bool isActive = _findActiveCommunicationPeriodService.Execute();
            ViewBag.IsActive = isActive; // ارسال نتیجه به ویو

            return View();
        }

        [HttpPost]
        public IActionResult Add(RequestMediaPerformancesDto requestMedia)
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


            if (requestMedia.Media == "1")
            {
                requestMedia.Media = "تلوزیون";
            }
            if (requestMedia.Media == "2")
            {
                requestMedia.Media = "رادیو";
            }


            if (user != null)
            {
                if (user.Operator == "Senior" && ModelState.IsValid)
                {
                    var operatorName = _getOperatorNameById.Execute(requestMedia.Operator);
                    _addMediaPerformancesService.Execute(new RequestMediaPerformances
                    {
                        Media = requestMedia.Media,
                        NetworkName = requestMedia.NetworkName,
                        ProgramName = requestMedia.ProgramName,
                        SubjectId = requestMedia.SubjectId,
                        BroadcastDate = requestMedia.BroadcastDate,
                        Time = requestMedia.Time,
                        Image = requestMedia.Image,
                        Operator = _getNameByNormalizedNameService.Execute(operatorName).Data,
                        BroadcastStartTime=requestMedia.BroadcastStartTime,
                        Description=requestMedia.Description
                        

                    });
                    return Json(new { isSuccess = true, message = "عملیات با موفقیت انجام شد." });
                }
                if (user.Operator != "Senior" && ModelState.IsValid)
                {
                    _addMediaPerformancesService.Execute(new RequestMediaPerformances
                    {
                        Media = requestMedia.Media,
                        NetworkName = requestMedia.NetworkName,
                        ProgramName = requestMedia.ProgramName,
                        SubjectId=requestMedia.SubjectId,
                        BroadcastDate = requestMedia.BroadcastDate,
                        Time = requestMedia.Time,
                        Image = requestMedia.Image,
                        Operator = _getNameByNormalizedNameService.Execute(user.Operator).Data,
                        BroadcastStartTime = requestMedia.BroadcastStartTime,
                        Description = requestMedia.Description

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
                var result = _getStatusRegistrationOperatorMediaService.Execute(new RequestStatusRegistrationMedia
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
            var result = _changeOfStatusMediaPerformancesService.Execute(id, user);

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
            var result = _getStatusMediaPerformances.Execute(id);
            if (result.IsSuccess)
            {
                return Json(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
        }

        public IActionResult DeleteMediaPerformances(int id)
        {
            var result = _deleteMediaPerformancesService.Execute(id);
            return Json(new
            {
                result.IsSuccess,
                result.Message
            });
        }

        public IActionResult PartialViewDetailMediaModal(int id)
        {
            var result = _getDetailMediaPerformanceService.Execute(id);
            return PartialView(result.Data);
        }

        public IActionResult PartialViewEditMediaModal(int id)
        {
            List<SelectListItem> Media = new List<SelectListItem>();
            Media.Add(new SelectListItem() { Text = "تلوزیون", Value = "1" });
            Media.Add(new SelectListItem() { Text = "رادیو", Value = "2" });

            ViewBag.Media = new SelectList(Media, "Value", "Text");
            ViewBag.Subjct = new SelectList(_getSubjectForDropDown.Execute().Data, "Id", "Name");


            var result = _getDetailMediaPerformanceService.Execute(id);
            return PartialView(result.Data);
        }

        [HttpPost]
        public IActionResult PartialViewEditMediaModal(RequestEditMediaDto request)
        {
            if (request.Media == "1")
            {
                request.Media = "تلوزیون";
            }
            if (request.Media == "2")
            {
                request.Media = "رادیو";
            }

            var result = _editMediaPerformancesService.Execute(new RequestEditMediaDto
            {
                Id = request.Id,
                Media = request.Media,
                NetworkName = request.NetworkName,
                ProgramName = request.ProgramName,
                SubjectId = request.SubjectId,
                Image = request.Image,
                Time=request.Time,
                BroadcastStartTime=request.BroadcastStartTime,
                BroadcastDate=request.BroadcastDate,
                Description=request.Description
            });
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
