using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.NewsPerformancesServices
{
    public interface IGetListNewsPerformancesService
    {
        ResultDto<ResultListNewsP> Execute(RequestListNewsP request);
    }
    public class ResultListNewsP
    {
        public List<NewsDto> newsDtos { get; set; }


        //Pageination
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class NewsDto
    {
        public int Id { get; set; }
        public string NewsAgencyName { get; set; }
        public string Subject { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Image { get; set; }
        public bool Confirmation { get; set; }
        public string Operator { get; set; }
    }

    public class RequestListNewsP
    {
        public string searchKey { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string NormalizedName { get; set; }
        public int CommunicationPeriodId { get; set; }
        public bool? ConfirmationStatus { get; set; } // افزودن فیلتر تایید


    }

    public class GetListNewsPerformancesService : IGetListNewsPerformancesService
    {
        private readonly IDataBaseContext _context;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

        public GetListNewsPerformancesService(IDataBaseContext context, IGetNameByNormalizedNameService getNameByNormalizedNameService)
        {
            _context = context;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
        }
        public ResultDto<ResultListNewsP> Execute(RequestListNewsP request)
        {
            if (request.NormalizedName == "Senior")
            {
                var result = _context.NewsPerformances.AsQueryable();
                int rowCount = 0;


                if (!string.IsNullOrWhiteSpace(request.searchKey))
                {
                    result = result.Where(p => p.NewsAgencyName.Contains(request.searchKey) ||
                     p.Subject.Contains(request.searchKey) || p.Operator.Contains(request.searchKey));
                }
                //------
                if (request.ConfirmationStatus != null)
                {
                    result = result.Where(p => p.Confirmation == request.ConfirmationStatus);
                }
                //------
                var newsPerformances = result
                    .Where(p => p.IsRemoved == false)
                    .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId)
                    .OrderByDescending(p => p.InsertTime)
                    .ToPaged(request.page, request.pageSize, out rowCount)
                    .Select(p => new NewsDto
                    {
                        Id = p.Id,
                        NewsAgencyName = p.NewsAgencyName,
                        Subject = p.Subject,
                        PublicationDate = p.PublicationDate,
                        Confirmation = p.Confirmation,
                        Image = p.Image,
                        Operator = p.Operator
                    }).ToList();
                return new ResultDto<ResultListNewsP>
                {
                    Data = new ResultListNewsP
                    {
                        newsDtos = newsPerformances,
                        RowCount = rowCount,
                        CurrentPage = request.page,
                        PageSize = request.pageSize
                    },
                    IsSuccess = true,
                    Message = ""
                };
            }
            else
            {
                if (request.NormalizedName != null)
                {
                    var result = _context.NewsPerformances.AsQueryable();
                    int rowCount = 0;


                    if (!string.IsNullOrWhiteSpace(request.searchKey))
                    {
                        result = result.Where(p => p.NewsAgencyName.Contains(request.searchKey) ||
                         p.Subject.Contains(request.searchKey));
                    }

                    var nameByNormalizedName = _getNameByNormalizedNameService.Execute(request.NormalizedName).Data;
                    //------
                    if (request.ConfirmationStatus != null)
                    {
                        result = result.Where(p => p.Confirmation == request.ConfirmationStatus);
                    }
                    //------
                    var newsPerformances = result
                        .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId)
                        .Where(p => p.Operator == nameByNormalizedName)
                        .OrderByDescending(p => p.InsertTime)
                        .ToPaged(request.page, request.pageSize, out rowCount)
                        .Select(p => new NewsDto
                        {
                            Id = p.Id,
                            NewsAgencyName = p.NewsAgencyName,
                            Subject = p.Subject,
                            PublicationDate = p.PublicationDate,
                            Confirmation = p.Confirmation,
                            Image = p.Image,
                            Operator = p.Operator
                        }).ToList();
                    return new ResultDto<ResultListNewsP>
                    {
                        Data = new ResultListNewsP
                        {
                            newsDtos = newsPerformances,
                            RowCount = rowCount,
                            CurrentPage = request.page,
                            PageSize = request.pageSize
                        },
                        IsSuccess = true,
                        Message = ""
                    };
                }
            }
            return new ResultDto<ResultListNewsP>
            {
                IsSuccess = false,
                Message = "خطایی هنگام پردازش رخ داده است"
            };
        }
    }

    //public class GetListNewsPerformancesService : IGetListNewsPerformancesService
    //{
    //    private readonly IDataBaseContext _context;
    //    private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

    //    public GetListNewsPerformancesService(IDataBaseContext context, IGetNameByNormalizedNameService getNameByNormalizedNameService)
    //    {
    //        _context = context;
    //        _getNameByNormalizedNameService = getNameByNormalizedNameService;
    //    }

    //    public ResultDto<ResultListNewsP> Execute(RequestListNewsP request)
    //    {
    //        var result = _context.NewsPerformances.AsQueryable();
    //        int rowCount = 0;

    //        if (!string.IsNullOrWhiteSpace(request.searchKey))
    //        {
    //            result = result.Where(p => p.NewsAgencyName.Contains(request.searchKey) ||
    //                                       p.Subject.Contains(request.searchKey) ||
    //                                       (request.NormalizedName == "Senior" ? p.Operator.Contains(request.searchKey) : false));
    //        }

    //        if (request.NormalizedName == "Senior")
    //        {
    //            result = result.Where(p => p.IsRemoved == false)
    //                           .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId);
    //        }
    //        else
    //        {
    //            if (request.NormalizedName != null)
    //            {
    //                var nameByNormalizedName = _getNameByNormalizedNameService.Execute(request.NormalizedName).Data;
    //                result = result.Where(p => p.Operator == nameByNormalizedName)
    //                               .Where(p => p.CommunicationPeriodId == request.CommunicationPeriodId);
    //            }
    //        }

    //        if (request.ConfirmationStatus != null)
    //        {
    //            result = result.Where(p => p.Confirmation == request.ConfirmationStatus);
    //        }

    //        var newsPerformances = result.OrderByDescending(p => p.InsertTime)
    //                                     .ToPaged(request.page, request.pageSize, out rowCount)
    //                                     .Select(p => new NewsDto
    //                                     {
    //                                         Id = p.Id,
    //                                         NewsAgencyName = p.NewsAgencyName,
    //                                         Subject = p.Subject,
    //                                         PublicationDate = p.PublicationDate,
    //                                         Confirmation = p.Confirmation,
    //                                         Image = p.Image,
    //                                         Operator = p.Operator
    //                                     }).ToList();

    //        return new ResultDto<ResultListNewsP>
    //        {
    //            Data = new ResultListNewsP
    //            {
    //                newsDtos = newsPerformances,
    //                RowCount = rowCount,
    //                CurrentPage = request.page,
    //                PageSize = request.pageSize
    //            },
    //            IsSuccess = true,
    //            Message = ""
    //        };
    //    }
    //}

}
