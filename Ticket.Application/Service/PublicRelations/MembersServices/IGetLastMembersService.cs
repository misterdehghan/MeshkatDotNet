
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MembersServices
{
    public interface IGetLastMembersService
    {
        ResultDto<int> Execute(int channelId);
    }

    public class GetLastMembersService : IGetLastMembersService
    {
        private readonly IDataBaseContext _context;

        public GetLastMembersService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<int> Execute(int channelId)
        {
            var result = _context.MembersPeriods
                .Where(p => p.ChannelId == channelId)
                .OrderByDescending(p => p.InsertTime)  // ترتیب نزولی بر اساس تاریخ ورود
                .FirstOrDefault();

            if (result != null)
            {
                return new ResultDto<int>
                {
                    IsSuccess = true,
                    Message = "",
                    Data = result.Member
                };
            }
            else
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "=هیچ رکوردی ثبت نشده است"

                };
            }


        }
    }
}
