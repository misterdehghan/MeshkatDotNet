using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.ChannelServices
{
    public interface IFindChannelService
    {
        ResultDto<ResultChannel> Execute(int channelId);
    }

    public class ResultChannel
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsRemoved { get; set; } = false;
        public DateTime? RemoveTime { get; set; }

        public string MessengersName { get; set; }
        public string ChannelName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public DateTime ActivationDate { get; set; }
        public string Operator { get; set; }
    }

    public class FindChannelService : IFindChannelService
    {
        private readonly IDataBaseContext _context;
        public FindChannelService(IDataBaseContext context)
        {
            _context = context;

        }

        public ResultDto<ResultChannel> Execute(int channelId)
        {
            var channel = _context.Channels.SingleOrDefault(c => c.Id == channelId);

            // بررسی اینکه آیا کانال پیدا شده است یا نه
            if (channel == null)
            {
                return new ResultDto<ResultChannel>
                {
                    IsSuccess = false,
                    Message = "کانال مورد نظر یافت نشد",
                    Data = null

                };
            }
            // ایجاد شیء ResultChannel از کانال پیدا شده
            var resultChannel = new ResultChannel
            {
                Id = channel.Id,
                InsertTime = channel.InsertTime,
                UpdateTime = channel.UpdateTime,
                IsRemoved = channel.IsRemoved,
                RemoveTime = channel.RemoveTime,
                MessengersName = channel.MessengersName,
                ChannelName = channel.ChannelName,
                PhoneNumber = channel.PhoneNumber,
                Address = channel.Address,
                ActivationDate = channel.ActivationDate,
                Operator = channel.Operator
            };

            // برگرداندن نتیجه موفقیت‌آمیز همراه با اطلاعات کانال
            return new ResultDto<ResultChannel>
            {
                IsSuccess = true,
                Message = "کانال یافت شد",
                Data = resultChannel
            };

        }
    }
}
