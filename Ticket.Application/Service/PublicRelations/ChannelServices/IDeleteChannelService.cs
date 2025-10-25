using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.ChannelServices
{
    public interface IDeleteChannelService
    {
        ResultDto Execute(int ChannelId);
    }

    public class DeleteChannelService : IDeleteChannelService
    {
        private readonly IDataBaseContext _context;
        public DeleteChannelService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(int ChannelId)
        {
            var channel = _context.Channels.FirstOrDefault(p => p.Id == ChannelId);


            // بررسی اینکه آیا کانال با این Id وجود دارد یا خیر
            if (channel == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کانال مورد نظر یافت نشد."
                };
            }


            if (channel != null)
            {
                channel.IsRemoved = true;
                channel.RemoveTime = DateTime.Now;


                _context.Channels.Update(channel);
                _context.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "کانال مورد نظر ّا موفقیت حذف شد."
                };
            }

            return new ResultDto()
            {
                IsSuccess = false,
                Message = "خطایی رخ داده است دوباره سعی کنید"
            };
        }
    }

}
