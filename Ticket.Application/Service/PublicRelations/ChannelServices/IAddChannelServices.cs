using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.PublicRelations.Main;
using System;

namespace Azmoon.Application.Service.PublicRelations.ChannelServices
{
    public interface IAddChannelServices
    {
        ResultDto Execute(ForAddChannlDto forAddChannlDto);
    }

    public class AddChannelServices : IAddChannelServices
    {
        private readonly IDataBaseContext _context;
        public AddChannelServices(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(ForAddChannlDto forAddChannlDto)
        {
            Channel channel = new Channel
            {
                Id = forAddChannlDto.Id,
                InsertTime = DateTime.Now,
                MessengersName = forAddChannlDto.MessengersName,
                ChannelName = forAddChannlDto.ChannelName,
                PhoneNumber = forAddChannlDto.PhoneNumber,
                Address = forAddChannlDto.Address,
                ActivationDate = forAddChannlDto.ActivationDate,
                Operator = forAddChannlDto.Operator,

            };
            _context.Channels.Add(channel);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "با موفقیت ذخیره شد"
            };
        }
    }


    public class ForAddChannlDto
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public string MessengersName { get; set; }
        public string ChannelName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime ActivationDate { get; set; }
        public string Operator { get; set; }
    }
}
