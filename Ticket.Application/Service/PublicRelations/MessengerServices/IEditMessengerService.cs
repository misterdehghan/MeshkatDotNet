
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MessengerServices
{
    public interface IEditMessengerService
    {
        ResultDto Execute(RequestEditMessengerDto request);
    }

    public class EditMessengerService : IEditMessengerService
    {
        private readonly IDataBaseContext _context;

        public EditMessengerService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditMessengerDto request)
        {
            var messenger = _context.Messengers.Find(request.Id);
            if (messenger == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "پیامرسان یافت نشد."
                };
            }
            messenger.PersianName = request.PersianName;
            messenger.LatinName = request.LatinName;
            messenger.UpdateTime = DateTime.Now;
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "ویرایش پیامرسان با موفقیت انجام شد."
            };
        }
    }

    public class RequestEditMessengerDto
    {
        public int Id { get; set; }
        public string PersianName { get; set; }
        public string LatinName { get; set; }
    }
}
