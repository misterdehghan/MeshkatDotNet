
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.ChannelServices
{
    public interface IEditChannelService
    {
        ResultDto Execute(ForChannelEdit channelEdit);
    }
    public class ForChannelEdit
    {
        public int Id { get; set; }

        public string MessengersName { get; set; }
        public string ChannelName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class EditChannelService : IEditChannelService
    {
        private readonly IDataBaseContext _context;
        public EditChannelService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(ForChannelEdit channelEdit)
        {
            var channel = _context.Channels.FirstOrDefault(p => p.Id == channelEdit.Id);

            // بررسی اینکه آیا کانال با این Id وجود دارد یا خیر
            if (channel == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کانال مورد نظر یافت نشد."
                };
            }

            // اگر MessengersName نال باشد، از نام پیام‌رسان قبلی استفاده می‌شود
            var messengersName = channelEdit.MessengersName ?? channel.MessengersName;


            if (channel != null)
            {
                channel.MessengersName = messengersName;
                channel.ChannelName = channelEdit.ChannelName;
                channel.PhoneNumber = channelEdit.PhoneNumber;
                channel.Address = channelEdit.Address;
                channel.UpdateTime = DateTime.Now;

                _context.Channels.Update(channel);
                _context.SaveChanges();
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "ویرایش با موفقیت انجام شد"
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
