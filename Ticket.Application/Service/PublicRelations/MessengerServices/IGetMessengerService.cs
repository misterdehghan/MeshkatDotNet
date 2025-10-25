using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System.Collections.Generic;
using System.Linq;



namespace Azmoon.Application.Service.PublicRelations.MessengerServices
{
    public interface IGetMessengerService
    {
        ResultDto<MessengerForAdmin> Execute(RequestMessengerDto request);
    }

    public class GetMessengerService : IGetMessengerService
    {
        private readonly IDataBaseContext _context;
        public GetMessengerService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<MessengerForAdmin> Execute(RequestMessengerDto request)
        {

            var result = _context.Messengers.AsQueryable();
            int rowCount = 0;

            var messengerList = result
                .ToPaged(request.page, request.pageSize, out rowCount)
                .Select(p => new MessengerDto
                {
                    Id = p.Id,
                    PersianName = p.PersianName,
                    LatinName = p.LatinName,

                }).ToList();


            return new ResultDto<MessengerForAdmin>
            {
                IsSuccess = true,
                Message = "",
                Data = new MessengerForAdmin
                {
                    messengerDto = messengerList,
                    CurrentPage = request.page,
                    PageSize = request.pageSize,
                    RowCount = rowCount
                }
            };
        }
    }

    public class MessengerForAdmin
    {
        public List<MessengerDto> messengerDto { get; set; }

        //Pageination
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class MessengerDto
    {
        public int Id { get; set; }
        public string PersianName { get; set; }
        public string LatinName { get; set; }
    }

    public class RequestMessengerDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
