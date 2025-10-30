using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.PublicRelations.Main.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;



namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IAddMediaPerformancesService
    {
        ResultDto Execute(RequestMediaPerformances requestMedia);
    }

    public class RequestMediaPerformances
    {

        public string Media { get; set; }
        public string NetworkName { get; set; }
        public string ProgramName { get; set; }
        public int SubjectId { get; set; }
        public DateTime BroadcastDate { get; set; }
        public TimeSpan BroadcastStartTime { get; set; } //ساعت پخش
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public IFormFile Image { get; set; }
        public string Operator { get; set; }
    }

    public class AddMediaPerformancesService : IAddMediaPerformancesService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public AddMediaPerformancesService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public ResultDto Execute(RequestMediaPerformances requestMedia)
        {

            // پیدا کردن دوره فعال (تنها یک دوره فعال وجود دارد)
            var activeStatistics = _context.CommunicationPeriod
                .Where(stat => stat.IsRemoved == false)
                .FirstOrDefault(stat => DateTime.Now >= stat.StartDate && DateTime.Now <= stat.EndDate); // بررسی تاریخ‌ها


            var subjectTitle= _context.Subjects.Where(p=>p.Id== requestMedia.SubjectId).FirstOrDefault().Title;
            if (activeStatistics != null)
            {

                var resultUpload = UploadFile(requestMedia.Image);

                MediaPerformance mediaPerformances = new MediaPerformance
                {
                    Media = requestMedia.Media,
                    NetworkName = requestMedia.NetworkName,
                    ProgramName = requestMedia.ProgramName,
                    SubjectId = requestMedia.SubjectId,
                    SubjectTitle = subjectTitle,
                    BroadcastDate = requestMedia.BroadcastDate,
                    BroadcastStartTime= requestMedia.BroadcastStartTime,
                    Description=requestMedia.Description,
                    Time = requestMedia.Time,
                    Image = resultUpload.FileNameAddress,
                    Confirmation = false,
                    InsertTime = DateTime.Now,
                    Operator = requestMedia.Operator,
                    CommunicationPeriodId = activeStatistics.Id

                };

                // اضافه کردن شی جدید به دیتابیس و ذخیره تغییرات
                _context.MediaPerformances.Add(mediaPerformances);
                _context.SaveChanges();

                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "با موفقیت ذخیره شد"
                };
            }


            return new ResultDto()
            {
                IsSuccess = false,
                Message = "هنگام پردازش مشکلی رخ داده است"
            };
        }

        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"Images\MediaPerformances\";
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


        //private UploadDto UploadAndResizeFile(IFormFile file, int width, int height)
        //{
        //    if (file != null && file.Length > 0)
        //    {
        //        string folder = $@"Images\MediaPerformances\";
        //        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);

        //        if (!Directory.Exists(uploadsRootFolder))
        //        {
        //            Directory.CreateDirectory(uploadsRootFolder);
        //        }

        //        string fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
        //        var filePath = Path.Combine(uploadsRootFolder, fileName);

        //        using (var stream = file.OpenReadStream())
        //        using (var originalImage = System.Drawing.Image.FromStream(stream))
        //        using (var resizedImage = new System.Drawing.Bitmap(width, height))
        //        {
        //            using (var graphics = System.Drawing.Graphics.FromImage(resizedImage))
        //            {
        //                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //                graphics.DrawImage(originalImage, 0, 0, width, height);
        //            }

        //            resizedImage.Save(filePath);
        //        }

        //        return new UploadDto
        //        {
        //            FileNameAddress = folder + fileName,
        //            Status = true
        //        };
        //    }

        //    return new UploadDto
        //    {
        //        FileNameAddress = "",
        //        Status = false
        //    };
        //}


    }


    public class UploadDto
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }


}
