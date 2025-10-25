using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.ChannelServices
{
    public interface IAddMembersServices
    {
        ResultDto Execute(ForAddMembersDto forAddMembersDto);
    }

    public class ForAddMembersDto
    {
        public int ChannelId { get; set; }
        public int StatisticsId { get; set; }
        public int Member { get; set; }
    }

    public class AddMembersServices : IAddMembersServices
    {
        private readonly IDataBaseContext _context;

        public AddMembersServices(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(ForAddMembersDto forAddMembersDto)
        {
            throw new NotImplementedException();
        }
    }
}
