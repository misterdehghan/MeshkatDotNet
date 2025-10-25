using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.Surves;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Questiones
{
    public class CrudQuestionSurvay : ICrudQuestionSurvay
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public CrudQuestionSurvay(IDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ResultDto<AddQuestionSurvayDto> Add(AddQuestionSurvayDto dto)
        {
            var model = new SurveyQuestion();
            model = _mapper.Map(dto, model);
            _context.SurveyQuestions.Add(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResultDto<AddQuestionSurvayDto>
                {
                    Data = _mapper.Map<AddQuestionSurvayDto>(model),
                    IsSuccess = true
                };
            }
            return new ResultDto<AddQuestionSurvayDto> { IsSuccess = false };
        }

        public ResultDto<AddQuestionSurvayDto> Edit(AddQuestionSurvayDto dto)
        {
            var model = _context.SurveyQuestions.Find(dto.Id);
            model = _mapper.Map(dto, model);
            _context.SurveyQuestions.Update(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResultDto<AddQuestionSurvayDto>
                {
                    Data = _mapper.Map<AddQuestionSurvayDto>(model),
                    IsSuccess = true
                };
            }
            return new ResultDto<AddQuestionSurvayDto> { IsSuccess = false };
        }

        public ResultDto<GetQuestionSurvayViewModel> FindById(long Id)
        {
            var model = _context.SurveyQuestions.AsNoTracking().Where(p => p.Id == Id).FirstOrDefault();


            if (model != null)
            {
                var result = _mapper.Map<GetQuestionSurvayViewModel>(model);
                return new ResultDto<GetQuestionSurvayViewModel>
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            return new ResultDto<GetQuestionSurvayViewModel> { IsSuccess = false };
        }

        public GetSurvayWithQuestionPager GetListPageineted(int page, int pageSize, long survayId)
        {
            var model = new GetSurvayWithQuestionPager();
            int rowCount = 0;
            var survayView = _context.Surveys.Include(p => p.Group).FirstOrDefault(p => p.Id == survayId);
            var resultsurvayView = _mapper.Map<GetSurvayDto>(survayView);
            var db = _context.SurveyQuestions.Where(p => p.Status == 1 && p.SurveyId == survayId).AsNoTracking().OrderByDescending(p => p.Id)
                .Include(p => p.SurveyAnswers).AsQueryable();
            model.GetSurvayDetiles = resultsurvayView;
            var paging = db.PagedResult(page, pageSize, out rowCount).ToList();
            var result = _mapper.Map<List<GetQuestionSurvayViewModel>>(paging);
            if (paging != null && paging.Count() > 0)
            {
                foreach (var item in paging)
                {
                    var countIsTrue = item.SurveyAnswers.Where(p => p.Status == 1).Count();
                    result.Where(p => p.Id == item.Id).FirstOrDefault().CountAsnswer = countIsTrue;
                }


                var pagingDto = new Common.ResultDto.PagingDto<List<GetQuestionSurvayViewModel>>(page, pageSize, rowCount, result);
                model.getQuestionWithPager = pagingDto;

                return model;
            }
            else
            {
                model.getQuestionWithPager = new Common.ResultDto.PagingDto<List<GetQuestionSurvayViewModel>>(page, pageSize, rowCount, null);

                return model;
            }

            //return new PagingDto<List<GetQuestionSurvayViewModel>>
            //{
            //    PageNo = page,
            //    PageSize = pageSize,
            //    TotalRecords = rowCount,
            //    Data = result
            //};
        }

        public ResultDto Remove(long Id)
        {
            var model = _context.SurveyQuestions.Find(Id);
            model.Status = 0;
            _context.SurveyQuestions.Update(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResultDto
                {

                    IsSuccess = true
                };
            }
            return new ResultDto { IsSuccess = false };
        }

        public ResultDto<GetQuestionDitelWithAnswersPagerInSurvay> GetQuestionDetailsWithAnswersPager(long questionId)
        {
            var result = new GetQuestionDitelWithAnswersPagerInSurvay() { };

            var questione = _context.SurveyQuestions.Where(p => p.Id == questionId)
            .AsNoTracking()
            .FirstOrDefault();

            if (questione != null)
            {
                var ansewers = _context.SurveyAnswers.Where(p => p.SurveyId == questionId && p.Status == 1).ToList();
                var QuestionModel = _mapper.Map<GetQuestionSurvayViewModel>(questione);
                
                var AnsweresModel = _mapper.Map<List<GetAnswerInSurvayViewModel>>(ansewers);


                result.GetQuestionDto = QuestionModel;
                result.getAnswers = AnsweresModel;
                return new ResultDto<GetQuestionDitelWithAnswersPagerInSurvay>
                {
                    Data = result,
                    IsSuccess = true,
                    Message = ""
                };

            }

            return new ResultDto<GetQuestionDitelWithAnswersPagerInSurvay>
            {
                Data = result,
                IsSuccess = false,
                Message = ""
            };
        }
    }
}
