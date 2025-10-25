using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.Result;
using Azmoon.Application.Service.Result.Dto;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmoon.Common.Useful;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Domain.Entities;

namespace Azmoon.Application.Service.Result.Query
{
    public class GetResultQuiz : IGetResultQuiz
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;
        private readonly IGetWorkplacFirstToEndParent _workplacFirstToEndParent;

        public GetResultQuiz(IDataBaseContext context, IMapper mapper, IGetWorkplacFirstToEndParent workplacFirstToEndParent)
        {
            _context = context;
            _mapper = mapper;
            _workplacFirstToEndParent = workplacFirstToEndParent;
        }
        public ResultDto<StimotReportQuizDto> getStimolReportQuizByQuizId(long id, string username)
        {
            var quiz = _context.Quizzes.AsNoTracking()
            .Where(p => p.Id == id)
            .FirstOrDefault();
            var model = _context.Results.AsNoTracking()
            .Where(p => p.Status == 1 && p.QuizId == id && p.StartQuiz >= quiz.StartDate && p.StartQuiz < quiz.EndDate)
            .Include(p => p.Student)
            .ThenInclude(p => p.WorkPlace)
                .Select(p => new QuizReportDro
                {
                    FullName = p.Student.FirstName + " " + p.Student.LastName,
                    Phone = p.Student.Phone,
                    WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent(p.Student.WorkPlace.Id).Data,
                    UserName = p.Student.UserName,
                    QuizStart = p.RegesterAt != null ? ((DateTime)p.RegesterAt).ToPersianDateStrFarsi() : "",
                    Darsad = (p.Points * 100) / p.MaxPoints,
                    Point = p.Points,

                })
                .ToList();
            if (model != null)
            {
          
                StimotReportQuizDto dto = new StimotReportQuizDto();
                dto.Name = quiz.Name;
                dto.QuizReports = model.OrderByDescending(p=>p.Point).ToList();
                dto.EndDate = quiz.EndDate != null ? ((DateTime)quiz.EndDate).ToPersianDateStrFarsi() : "";
                dto.StartDate = quiz.StartDate != null ? ((DateTime)quiz.StartDate).ToPersianDateStrFarsi() : "";
                dto.QuizNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "/" + quiz.Id + "/" + username;
                return new ResultDto<StimotReportQuizDto>
                {

                    Data = dto,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            return new ResultDto<StimotReportQuizDto>
            {
                Data = null,
                IsSuccess = false,
                Message = "Warninge"
            };
        }
        public ResultDto<AddResultQuizDto> getResultByQuizId(long id)
        {
            var model = _context.Results.Where(p => p.Status == 1 && p.Id == id)
           .AsNoTracking()
           .FirstOrDefault();
            if (model != null)
            {
                var result = _mapper.Map<AddResultQuizDto>(model);

                return new ResultDto<AddResultQuizDto>
                {

                    Data = result,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            return new ResultDto<AddResultQuizDto>
            {
                Data = null,
                IsSuccess = false,
                Message = "Warninge"
            };
        }
        public ResultDto<StimotReportQuizDto> getResultLottery(long id, int count, int min, int max ,string username)
        {
            var quiz = _context.Quizzes.AsNoTracking()
            .Where(p => p.Id == id)
            .FirstOrDefault();
            var model = _context.Results.Where(p => p.Status == 1 && p.QuizId == id &&
            p.Points >= min && p.Points <= max &&
            p.StartQuiz >= quiz.StartDate && p.StartQuiz < quiz.EndDate)
           .AsNoTracking()
           .Include(p => p.Student)
           .ThenInclude(p => p.WorkPlace)
              .Select(p => new QuizReportDro
              {
                  FullName = p.Student.FirstName + " " + p.Student.LastName,
                  Phone = p.Student.Phone,
                  WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent(p.Student.WorkPlace.Id).Data,
                  UserName = p.Student.UserName,
                  QuizStart = p.RegesterAt != null ? ((DateTime)p.RegesterAt).ToPersianDateStrFarsi() : "",
                  Darsad = (p.Points * 100) / p.MaxPoints,
                  Point = p.Points,
                  NumberBankAccunt=p.Student.NumberBankAccunt,

              })
               .OrderByDescending(p => p.Point).ToList();
            if (model != null && model.Count() > 0)
            {
           
                StimotReportQuizDto dto = new StimotReportQuizDto();
                dto.Name = quiz.Name;
           
                dto.EndDate = quiz.EndDate != null ? ((DateTime)quiz.EndDate).ToPersianDateStrFarsi() : "";
                dto.StartDate = quiz.StartDate != null ? ((DateTime)quiz.StartDate).ToPersianDateStrFarsi() : "";
                dto.QuizNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "/" + quiz.Id + "/" + username;
                if (model.Count() <= count)
                {

                    dto.QuizReports = model;
                    return new ResultDto<StimotReportQuizDto>
                    {

                        Data = dto,
                        IsSuccess = true,
                        Message = "Success"
                    };
                }
                else
                {
                    List<QuizReportDro > results = new List<QuizReportDro>();
                    var arryIndex = getArreyIndex(model.Count() , count);
                    for (int i = 0; i < count; i++)
                    {
                        results.Add(model.ElementAt(arryIndex[i]));
                    }

                    dto.QuizReports = results;
                    return new ResultDto<StimotReportQuizDto>
                    {

                        Data = dto,
                        IsSuccess = true,
                        Message = "Success"
                    };
                }

            }
            return new ResultDto<StimotReportQuizDto>
            {
                Data = null,
                IsSuccess = false,
                Message = "Warninge"
            };
        }
        public ResultDto<GetResutQuizMyWithPager> getResultByUserId(int PageNo, int PageSize, int status, string UserId)
        {
            var date = new GetResutQuizMyWithPager() { };
            int rowCount = 0;
            var model = _context.Results.Where(p => p.Status == status)
                .AsNoTracking()
                .Include(p => p.Student)
                .Include(p => p.Quiz)
                .AsQueryable();
            if (!String.IsNullOrEmpty(UserId))
            {
                model = model.Where(p => p.StudentId == UserId.Trim())

               .AsQueryable();
            }


            if (model != null)
            {

                var paging = model.ToPaged(PageNo, PageSize, out rowCount).ToList();
                 var result = _mapper.Map<List<GetResutQuizDto>>(paging);
        
                date.ResultQuizDtos = result;
                date.PagerDto = new PagerDto
                {
                    PageNo = PageNo,
                    PageSize = PageSize,
                    TotalRecords = rowCount
                };
                return new ResultDto<GetResutQuizMyWithPager>
                {

                    Data = date,
                    IsSuccess = true,
                    Message = "Success"
                };
            }

            return new ResultDto<GetResutQuizMyWithPager>
            {
                Data = null,
                IsSuccess = false,
                Message = "Warninge"
            };
        }

        public ResultDto<GetResutQuizWithPager> getResultWithPager(int PageSize, int PageNo, string searchKey, int status, long guizId)
        {
            var date = new GetResutQuizWithPager() { };
            int rowCount = 0;
            var quizz = _context.Quizzes.AsNoTracking().Where(p => p.Id == guizId).FirstOrDefault();
            var model = _context.Results.AsNoTracking().Where(p => p.Status == status && p.QuizId == guizId && p.StartQuiz>= quizz.StartDate && p.StartQuiz < quizz.EndDate)
               .OrderByDescending(p => p.Points)
                  .Include(p => p.Student)
                  .ThenInclude(p => p.WorkPlace)
                .AsQueryable();
            if (!String.IsNullOrEmpty(searchKey))
            {




                var studentsId = _context.Users.AsNoTracking().Where(p => p.FirstName.Contains(searchKey.Trim()) ||
                p.LastName.Contains(searchKey.Trim()) ||
                  p.UserName.Contains(searchKey.Trim())
                ).Select(o => o.Id).ToList();
                if (studentsId.Count()>0)
                {
                    model = model.Where(p => studentsId.Contains( p.StudentId)).AsQueryable();

                }
            }


            if (model != null)
            {
              
                var paging = model.ToPaged(PageNo, PageSize, out rowCount).ToList();
                var result = paging.Select(p => new QuizReportDro
                {
                    Id = p.Id,
                    FullName = p.Student.FirstName + " " + p.Student.LastName,
                    Phone = p.Student.Phone,
                    WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent(p.Student.WorkPlace.Id).Data,
                    UserName = p.Student.UserName,
                    QuizStart = p.RegesterAt != null ? ((DateTime)p.RegesterAt).ToPersianDateStrFarsi() : "",
                    Darsad = (p.Points * 100) / p.MaxPoints,
                    Point = p.Points,

                }).ToList();
                // var result = _mapper.Map<List<QuizReportDro>>(paging);
                for (int i = 0; i < result.Count(); i++)
                {
                    result.ElementAt(i).WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent(paging.FirstOrDefault(p=>p.Id== result.ElementAt(i).Id).Student.WorkPlace.Id).Data;
                }
                date.ResultQuizDtos = result;
                date.PagerDto = new PagerDto
                {
                    PageNo = PageNo,
                    PageSize = PageSize,
                    TotalRecords = rowCount
                };
                return new ResultDto<GetResutQuizWithPager>
                {

                    Data = date,
                    IsSuccess = true,
                    Message = "Success"
                };
            }

            return new ResultDto<GetResutQuizWithPager>
            {
                Data = null,
                IsSuccess = false,
                Message = "Warninge"
            };
        }

        private IList<int> getArreyIndex(int length, int count)
        {

            Random rndQusetionIndex = new Random();

            IList<int> result = new List<int>();
            do
            {
                var a = rndQusetionIndex.Next(0, (length - 1));
                if (!result.Where(p => p == a).Any())
                {
                    result.Add(a);
                }

            }
            while (result.Count() <= count);

            return result;
        }

        public List<QuizReportDro> getQuizRezultXLSX(string searchKey, int status, long guizId)
            {
            var quiz = _context.Quizzes
               .Where(p => p.Id == guizId)
               .AsNoTracking()
               .FirstOrDefault();
            var model = _context.Results.AsNoTracking().Where(p => p.Status == status && p.QuizId == guizId && p.StartQuiz > quiz.StartDate && p.StartQuiz < quiz.EndDate)
               .OrderByDescending(p => p.Points)
                  .Include(p => p.Student)
                  .ThenInclude(p => p.WorkPlace)
                .AsQueryable();
            if (!String.IsNullOrEmpty(searchKey))
                {




                var studentsId = _context.Users.AsNoTracking().Where(p => p.FirstName.Contains(searchKey.Trim()) ||
                p.LastName.Contains(searchKey.Trim()) ||
                  p.UserName.Contains(searchKey.Trim())
                ).Select(o => o.Id).ToList();
                if (studentsId.Count() > 0)
                    {
                    model = model.Where(p => studentsId.Contains(p.StudentId)).AsQueryable();

                    }
                }


            if (model != null)
                {

                var paging = model.ToList();
                var result = paging.Select(p => new QuizReportDro
                    {
                    Id = p.Id,
                    FullName = p.Student.FirstName + " " + p.Student.LastName, 
                    melli = p.Student.melli,
                    Phone = p.Student.Phone,
                    WorkPlaceName = _workplacFirstToEndParent.FirstToEndParent(p.Student.WorkPlace.Id).Data,
                    UserName = p.Student.UserName,
                    QuizStart = p.RegesterAt != null ? ((DateTime)p.RegesterAt).ToPersianDateStrFarsi() : "",
                    Darsad = (p.Points * 100) / p.MaxPoints,
                    Point = p.Points,
                    NumberBankAccunt=p.Student.NumberBankAccunt
                    }).ToList();
                return result;
                }
            return null;

            }

        }
}
