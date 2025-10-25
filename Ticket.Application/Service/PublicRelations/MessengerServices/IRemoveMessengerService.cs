using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MessengerServices
{
    public interface IRemoveMessengerService
    {
        ResultDto Execute(int MessengerId);
    }

    public class RemoveMessengerService : IRemoveMessengerService
    {
        private readonly IDataBaseContext _context;

        public RemoveMessengerService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int MessengerId)
        {
            var messenger = _context.Messengers.Find(MessengerId);
            if (messenger == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "پیامرسان یافت نشد"
                };
            }
            messenger.IsRemoved = true;
            messenger.RemoveTime = DateTime.Now;
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "پیامرسان با موفقیت حذف شد"
            };
        }
    }
}
