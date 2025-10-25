using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Azmoon.Common.ResultDto;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Domain.Entities.PublicRelations.Main.News;


namespace Azmoon.Application.Service.PublicRelations.NewsPerformancesServices
{
    public interface IAddNewsPerformancesService
    {
        ResultDto Execute(RequestNewsPerformances request);
    }

    public class RequestNewsPerformances
    {
        public string NewsAgencyName { get; set; }
        public string Subject { get; set; }
        public DateTime PublicationDate { get; set; }
        public IFormFile Image { get; set; }
        public string Operator { get; set; }
    }

    public class AddNewsPerformancesService : IAddNewsPerformancesService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public AddNewsPerformancesService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public ResultDto Execute(RequestNewsPerformances request)
        {

            // پیدا کردن دوره فعال (تنها یک دوره فعال وجود دارد)
            var activeStatistics = _context.CommunicationPeriod
                .Where(stat => stat.IsRemoved == false)
                .FirstOrDefault(stat => DateTime.Now >= stat.StartDate && DateTime.Now <= stat.EndDate); // بررسی تاریخ‌ها

            if (activeStatistics != null)
            {
                var resultUpload = UploadFile(request.Image);


                NewsPerformances newsPerformances = new NewsPerformances
                {
                    InsertTime = DateTime.Now,
                    NewsAgencyName = request.NewsAgencyName,
                    Subject = request.Subject,
                    PublicationDate = request.PublicationDate,
                    Image = resultUpload.FileNameAddress,
                    Confirmation = false,
                    Operator = request.Operator,
                    CommunicationPeriodId = activeStatistics.Id
                };

                // اضافه کردن شی جدید به دیتابیس و ذخیره تغییرات
                _context.NewsPerformances.Add(newsPerformances);
                _context.SaveChanges();

                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "با موفقیت ذخیره شد"
                };
            }
            else
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "هنگام پردازش مشکلی رخ داده است"
                };
            }



        }




        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"Images\NewsPerformances\";
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }


                if (file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadsRootFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new UploadDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                };
            }
            return null;
        }
    }


    public class UploadDto
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }
}
