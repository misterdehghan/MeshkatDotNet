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
        public string Subject { get; set; }
        public IFormFile Image { get; set; }
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
            if (request.Media == "null" &&
                request.NetworkName == media.NetworkName &&
                request.ProgramName == media.ProgramName &&
                request.Subject == media.Subject &&
                request.Image == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما هیچ تغییری ایجاد نکرده اید."
                };
            }
            if (request.Image != null)
            {
                var resultUpload = UploadFile(request.Image);
                if (resultUpload != null)
                {
                    media.Image = resultUpload.FileNameAddress;
                }
            }


            if (request.Media == "null")
            {
                media.Media = media.Media;
            }
            else{
                media.Media = request.Media;
            }
            media.NetworkName = request.NetworkName ?? media.NetworkName;
            media.ProgramName = request.ProgramName ?? media.ProgramName;
            media.Subject = request.Subject ?? media.Subject;
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



