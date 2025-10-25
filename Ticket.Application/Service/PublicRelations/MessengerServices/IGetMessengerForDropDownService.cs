using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MessengerServices
{
    public interface IGetMessengerForDropDownService
    {
        ResultDto<List<MessegerForDropDownDto>> Execute();

    }

    public class GetMessengerForDropDownService : IGetMessengerForDropDownService
    {
        private readonly IDataBaseContext _context;
        public GetMessengerForDropDownService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<MessegerForDropDownDto>> Execute()
        {
            var Messenger = _context.Messengers.ToList().Select(p => new MessegerForDropDownDto
            {
                Id = p.Id,
                Name = p.PersianName + " - " + p.LatinName
            }).ToList();

            return new ResultDto<List<MessegerForDropDownDto>>
            {
                IsSuccess = true,
                Data = Messenger,
                Message = ""
            };
        }

    }

    public class MessegerForDropDownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
