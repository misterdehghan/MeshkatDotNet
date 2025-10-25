using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IGetListMediaPerformancesService
    {
        ResultDto<ResultListMP> Execute(RequestListMPDto request);
    }

    public class ResultListMP
    {
        public List<MediaDto> mediaDto { get; set; }

        //Pageination
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class MediaDto
    {
        public int Id { get; set; }
        public string Media { get; set; }
        public string NetworkName { get; set; }
        public string ProgramName { get; set; }
        public string Subject { get; set; }
        public DateTime BroadcastDate { get; set; }
        public TimeSpan Time { get; set; }
        public string Image { get; set; }
        public bool Confirmation { get; set; }
        public string Operator { get; set; }

    }

    public class RequestListMPDto
    {
        public string searchKey { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string NormalizedName { get; set; }
        public int CommunicationPeriodId { get; set; }
        public bool? ConfirmationStatus { get; set; } // افزودن فیلتر تایید
    }


    public class GetListMediaPerformancesService : IGetListMediaPerformancesService
    {
        private readonly IDataBaseContext _context;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

        public GetListMediaPerformancesService(IDataBaseContext context, IGetNameByNormalizedNameService getNameByNormalizedNameService)
        {
            _context = context;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
        }
        public ResultDto<ResultListMP> Execute(RequestListMPDto request)
        {
            if (request.NormalizedName == "Senior")
            {
                var result = _context.MediaPerformances.AsQueryable();
                int rowCount = 0;

                if (!string.IsNullOrWhiteSpace(request.searchKey))
                {
                    result = result.Where(p => p.NetworkName.Contains(request.searchKey) ||
                    p.ProgramName.Contains(request.searchKey) ||
                    p.Subject.Contains(request.searchKey) ||
                    p.Operator.Contains(request.searchKey));
                }
                //------
                if (request.ConfirmationStatus != null)
                {
                    result = result.Where(p => p.Confirmation == request.ConfirmationStatus);
                }
                //------
                var mediaPerformances = result
               .Where(p => p.IsRemoved == false)
               .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId)
               .OrderByDescending(p => p.InsertTime)
               .ToPaged(request.page, request.pageSize, out rowCount)
               .Select(p => new MediaDto
               {
                   Media = p.Media,
                   NetworkName = p.NetworkName,
                   ProgramName = p.ProgramName,
                   Subject = p.Subject,
                   BroadcastDate = p.BroadcastDate,
                   Time = p.Time,
                   Image = p.Image,
                   Confirmation = p.Confirmation,
                   Operator = p.Operator,
                   Id = p.Id
               }).ToList();


                return new ResultDto<ResultListMP>
                {
                    Data = new ResultListMP
                    {
                        mediaDto = mediaPerformances,
                        CurrentPage = request.page,
                        PageSize = request.pageSize,
                        RowCount = rowCount,
                    },
                    IsSuccess = true,
                    Message = ""

                };
            }
            else
            {
                if (request.NormalizedName != null)
                {
                    var result = _context.MediaPerformances.AsQueryable();
                    int rowCount = 0;

                    if (!string.IsNullOrWhiteSpace(request.searchKey))
                    {
                        result = result.Where(p => p.NetworkName.Contains(request.searchKey) ||
                        p.ProgramName.Contains(request.searchKey) ||
                        p.Subject.Contains(request.searchKey) ||
                        p.Operator.Contains(request.searchKey));
                    }
                    var nameByNormalizedName = _getNameByNormalizedNameService.Execute(request.NormalizedName).Data;


                    //------
                    if (request.ConfirmationStatus != null)
                    {
                        result = result.Where(p => p.Confirmation == request.ConfirmationStatus);
                    }
                    //------

                    var mediaPerformances = result
                   .Where(p => p.IsRemoved == false)
                   .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId)
                   .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId)
                   .Where(p => p.Operator == nameByNormalizedName)
                   .OrderByDescending(p => p.InsertTime)
                   .ToPaged(request.page, request.pageSize, out rowCount)
                   .Select(p => new MediaDto
                   {
                       Media = p.Media,
                       NetworkName = p.NetworkName,
                       ProgramName = p.ProgramName,
                       Subject = p.Subject,
                       BroadcastDate = p.BroadcastDate,
                       Time = p.Time,
                       Image = p.Image,
                       Confirmation = p.Confirmation,
                       Operator = p.Operator,
                       Id = p.Id
                   }).ToList();


                    return new ResultDto<ResultListMP>
                    {
                        Data = new ResultListMP
                        {
                            mediaDto = mediaPerformances,
                            CurrentPage = request.page,
                            PageSize = request.pageSize,
                            RowCount = rowCount,
                        },
                        IsSuccess = true,
                        Message = ""

                    };
                }
            }
            return new ResultDto<ResultListMP>
            {
                IsSuccess = false,
                Message = "خطایی هنگام پردازش رخ داده است"

            };




        }
    }
}