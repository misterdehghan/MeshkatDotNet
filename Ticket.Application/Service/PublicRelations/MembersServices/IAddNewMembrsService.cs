using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.PublicRelations.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MembersServices
{
    public interface IAddNewMembrsService
    {
        ResultDto Execute(RequestMembrsDto request);
    }



    public class AddNewMembrsService : IAddNewMembrsService
    {
        private readonly IDataBaseContext _context;
        public AddNewMembrsService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestMembrsDto request)
        {

            MembersPeriod membersPeriod = new MembersPeriod
            {
                Member = request.Member,
                ChannelId = request.ChannelId,
                VirtualSpacePeriodId = request.VirtualSpacePeriodId,
                InsertTime = DateTime.Now,
                IsRemoved = false

            };
            _context.MembersPeriods.Add(membersPeriod);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "با موفقیت ذخیره شد"
            };
        }
    }

    public class RequestMembrsDto
    {
        public int Member { get; set; }
        public int ChannelId { get; set; }
        public int VirtualSpacePeriodId { get; set; }
    }

}
