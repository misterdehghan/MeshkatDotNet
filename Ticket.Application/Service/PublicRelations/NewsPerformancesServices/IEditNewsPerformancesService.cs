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

namespace Azmoon.Application.Service.PublicRelations.NewsPerformancesServices
{
    public interface IEditNewsPerformancesService
    {
        ResultDto Execute(RequestEditNewsDto request);
    }

    public class RequestEditNewsDto
    {
        public int Id { get; set; }

        public string NewsAgencyName { get; set; }
        public string Subject { get; set; }
        public IFormFile Image { get; set; }
    }

    public class EditNewsPerformancesService : IEditNewsPerformancesService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public EditNewsPerformancesService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        public ResultDto Execute(RequestEditNewsDto request)
        {
            var news = _context.NewsPerformances.FirstOrDefault(p => p.Id == request.Id);
            if (news == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رکورد مورد نظر یافت نشد."
                };

            }
            if (request.NewsAgencyName == news.NewsAgencyName && request.Subject == news.Subject && request.Image == null)
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
                    news.Image = resultUpload.FileNameAddress;
                }
            }


            news.NewsAgencyName = request.NewsAgencyName ?? news.NewsAgencyName;
            news.Subject = request.Subject ?? news.Subject;
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
}
