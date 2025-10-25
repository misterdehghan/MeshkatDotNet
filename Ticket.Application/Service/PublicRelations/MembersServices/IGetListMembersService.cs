using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MembersServices
{
    public interface IGetListMembersService
    {
        ResultDto<ResultListMembersDto> Execute(RequestListMembersDto request);
    }

    public class GetListMembersService : IGetListMembersService
    {
        private readonly IDataBaseContext _context;
        public GetListMembersService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultListMembersDto> Execute(RequestListMembersDto request)
        {

            var result = _context.MembersPeriods
                .Include(p => p.VirtualSpacePeriod)
                .Where(p => p.ChannelId == request.ChannelId).ToList(); ;

            var periodsList = result
            .OrderByDescending(p => p.InsertTime)
           .Select(p => new MembersListDto
           {
               InsertTime = p.InsertTime,
               Member = p.Member,
               UpdateTime = p.UpdateTime,
               Statistics = p.VirtualSpacePeriod.StatisticalPeriod,

           }).ToList();

            // دریافت نام کانال
            var channelName = _context.Channels
                .Where(c => c.Id == request.ChannelId)
                .Select(c => c.ChannelName)
                .FirstOrDefault();

            return new ResultDto<ResultListMembersDto>
            {
                IsSuccess = true,
                Message = "",
                Data = new ResultListMembersDto
                {
                    listDtos = periodsList,
                    ChannelName = channelName,
                }
            };



        }
    }

    public class ResultListMembersDto
    {
        public List<MembersListDto> listDtos { get; set; }
        public string ChannelName { get; set; }

    }

    public class MembersListDto
    {
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Member { get; set; }
        public string Statistics { get; set; }
    }


    public class RequestListMembersDto
    {
        public int ChannelId { get; set; }
    }

}
