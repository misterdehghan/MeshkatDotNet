using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.ChannelServices
{
    public interface IGetListChannelServices
    {
        ResultDto<ReslutGetChannelDto> Execute(RequestGetChannelDto request);
    }

    public class GetListChannelServices : IGetListChannelServices
    {
        private readonly IDataBaseContext _context;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

        public GetListChannelServices(IDataBaseContext context, IGetNameByNormalizedNameService getNameByNormalizedNameService)
        {
            _context = context;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
        }

        public ResultDto<ReslutGetChannelDto> Execute(RequestGetChannelDto request)
        {
            if (request.Operator == "Senior")
            {
                var channels = _context.Channels.AsQueryable();

                // اضافه کردن شرط IsRemove برابر با false
                channels = channels.Where(p => p.IsRemoved == false);

                if (!string.IsNullOrEmpty(request.searchKey))
                {
                    channels = channels.Where(p => p.MessengersName.Contains(request.searchKey) ||
                    p.PhoneNumber.Contains(request.searchKey) ||
                    p.ChannelName.Contains(request.searchKey) ||
                    p.Operator.Contains(request.searchKey) ||
                    p.Address.Contains(request.searchKey));
                }

                int rowsCount = 0;
                var channelsList = channels.OrderByDescending(p => p.InsertTime)
                    .ToPaged(request.page, request.pageSize, out rowsCount).Select(p => new GetChannelsDto
                    {
                        Id = p.Id,
                        InsertTime = p.InsertTime,
                        UpdateTime = p.UpdateTime,
                        MessengersName = p.MessengersName,
                        ChannelName = p.ChannelName,
                        PhoneNumber = p.PhoneNumber,
                        Address = p.Address,
                        ActivationDate = p.ActivationDate,
                        Operator = p.Operator
                    }).ToList();
                return new ResultDto<ReslutGetChannelDto>
                {
                    Data = new ReslutGetChannelDto
                    {
                        Channels = channelsList,
                        RowCount = rowsCount,
                        CurrentPage = request.page,
                        PageSize = request.pageSize
                    },
                    IsSuccess = true,
                    Message = ""
                };
            }
            else
            {
                if (request.Operator != null)
                {
                    var channels = _context.Channels.AsQueryable();

                    // اضافه کردن شرط IsRemove برابر با false
                    channels = channels.Where(p => p.IsRemoved == false);


                    if (!string.IsNullOrEmpty(request.searchKey))
                    {
                        channels = channels.Where(p => p.MessengersName.Contains(request.searchKey) ||
                        p.PhoneNumber.Contains(request.searchKey) ||
                        p.ChannelName.Contains(request.searchKey) ||
                        p.Address.Contains(request.searchKey));
                    }
                    int rowsCount = 0;

                    var nameByNormalizedName = _getNameByNormalizedNameService.Execute(request.Operator).Data;

                    var channelsList = channels
                        .Where(p => p.Operator == nameByNormalizedName)
                        .ToPaged(request.page, request.pageSize, out rowsCount)
                        .Select(p => new GetChannelsDto
                        {
                            Id = p.Id,
                            InsertTime = p.InsertTime,
                            UpdateTime = p.UpdateTime,
                            MessengersName = p.MessengersName,
                            ChannelName = p.ChannelName,
                            PhoneNumber = p.PhoneNumber,
                            Address = p.Address,
                            ActivationDate = p.ActivationDate,
                            Operator = p.Operator

                        }).ToList();
                    return new ResultDto<ReslutGetChannelDto>
                    {
                        Data = new ReslutGetChannelDto
                        {
                            Channels = channelsList,
                            RowCount = rowsCount,
                            CurrentPage = request.page,
                            PageSize = request.pageSize
                        },
                        IsSuccess = true,
                        Message = ""
                    };
                }
                return new ResultDto<ReslutGetChannelDto>
                {

                    IsSuccess = false,
                    Message = ""
                };
            }
        }
    }

    public class ReslutGetChannelDto
    {
        public List<GetChannelsDto> Channels { get; set; }

        //Pageination
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class GetChannelsDto
    {
        public int Id { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string MessengersName { get; set; }
        public string ChannelName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime ActivationDate { get; set; }
        public string Operator { get; set; }
    }

    public class RequestGetChannelDto
    {
        public string Operator { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string searchKey { get; set; }
    }
}
