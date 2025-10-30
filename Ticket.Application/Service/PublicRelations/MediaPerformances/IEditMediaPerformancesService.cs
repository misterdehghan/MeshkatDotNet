using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IEditMediaPerformancesService
    {
        ResultDto Execute(RequestEditMediaDto request);
    }

    public class RequestEditMediaDto
    {
        public int Id { get; set; }
        public string Media { get; set; }
        public string NetworkName { get; set; }
        public string ProgramName { get; set; }
        public IFormFile Image { get; set; }


        public int SubjectId { get; set; }
        public DateTime BroadcastDate { get; set; }
        public TimeSpan BroadcastStartTime { get; set; } //ساعت پخش
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
       
        
    }

    public class EditMediaPerformancesService : IEditMediaPerformancesService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public EditMediaPerformancesService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public ResultDto Execute(RequestEditMediaDto request)
        {
            var media = _context.MediaPerformances.FirstOrDefault(p => p.Id == request.Id);
            if (media == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رکورد مورد نظر یافت نشد."
                };
            }

            bool hasChanges = false;

            // آپلود فایل اگر تغییر کرده
            if (request.Image != null)
            {
                var resultUpload = UploadFile(request.Image);
                if (resultUpload != null)
                {
                    media.Image = resultUpload.FileNameAddress;
                    hasChanges = true;
                }
            }

            // بررسی تغییرات هر فیلد
            if (!string.IsNullOrEmpty(request.Media) && media.Media != request.Media)
            {
                media.Media = request.Media;
                hasChanges = true;
            }

            if (request.SubjectId > 0)
            {
                var subjectTitle = _context.Subjects.FirstOrDefault(p => p.Id == request.SubjectId)?.Title;
                if (subjectTitle != null && media.SubjectTitle != subjectTitle)
                {
                    media.SubjectTitle = subjectTitle;
                    hasChanges = true;
                }
            }

            if (request.BroadcastDate != DateTime.MinValue &&
                request.BroadcastDate.Year > 1 &&
                media.BroadcastDate != request.BroadcastDate)
            {
                media.BroadcastDate = request.BroadcastDate;
                hasChanges = true;
            }

            if (request.Time != TimeSpan.Zero && media.Time != request.Time)
            {
                media.Time = request.Time;
                hasChanges = true;
            }

            if (request.BroadcastStartTime != TimeSpan.Zero && media.BroadcastStartTime != request.BroadcastStartTime)
            {
                media.BroadcastStartTime = request.BroadcastStartTime;
                hasChanges = true;
            }

            if (!string.IsNullOrEmpty(request.NetworkName) && media.NetworkName != request.NetworkName)
            {
                media.NetworkName = request.NetworkName;
                hasChanges = true;
            }

            if (!string.IsNullOrEmpty(request.ProgramName) && media.ProgramName != request.ProgramName)
            {
                media.ProgramName = request.ProgramName;
                hasChanges = true;
            }

            if (!string.IsNullOrEmpty(request.Description) && media.Description != request.Description)
            {
                media.Description = request.Description;
                hasChanges = true;
            }

            // اگر تغییری نبود
            if (!hasChanges)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تغییراتی ایجاد نشد."
                };
            }

            // ذخیره تغییرات
            media.UpdateTime = DateTime.Now;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ویرایش با موفقیت انجام شد."
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
    }
}



