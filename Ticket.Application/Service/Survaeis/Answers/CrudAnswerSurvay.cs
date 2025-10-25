using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
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

namespace Azmoon.Application.Service.Survaeis.Answers
{
    public class CrudAnswerSurvay : ICrudAnswerSurvay
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public CrudAnswerSurvay(IDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ResultDto<AddAnswerSurvayDto> Add(AddAnswerSurvayDto dto)
        {
            var model = new SurveyAnswer();
            model = _mapper.Map(dto, model);
            _context.SurveyAnswers.Add(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResultDto<AddAnswerSurvayDto>
                {
                    Data = _mapper.Map<AddAnswerSurvayDto>(model),
                    IsSuccess = true
                };
            }
            return new ResultDto<AddAnswerSurvayDto> { IsSuccess = false };
        }

        public ResultDto<AddAnswerSurvayDto> Edit(AddAnswerSurvayDto dto)
        {
            var model = _context.SurveyAnswers.Find(dto.Id);
            model = _mapper.Map(dto, model);
            _context.SurveyAnswers.Update(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResultDto<AddAnswerSurvayDto>
                {
                    Data = _mapper.Map<AddAnswerSurvayDto>(model),
                    IsSuccess = true
                };
            }
            return new ResultDto<AddAnswerSurvayDto> { IsSuccess = false };
        }
        public ResultDto<AddAnswerSurvayDto> EditKeyAnswer(EditFeature_dto dto)
        {
            var model = _context.SurveyAnswers.Find(dto.Id);
            model = _mapper.Map(dto, model);
            _context.SurveyAnswers.Update(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResultDto<AddAnswerSurvayDto>
                {
                    Data = _mapper.Map<AddAnswerSurvayDto>(model),
                    IsSuccess = true
                };
            }
            return new ResultDto<AddAnswerSurvayDto> { IsSuccess = false };
        }
        public ResultDto<GetAnswerSurvayViewModel> FindById(int Id)
        {
            var model = _context.SurveyAnswers.AsNoTracking().Where(p => p.Id == Id).FirstOrDefault();


            if (model != null)
            {
                var result = _mapper.Map<GetAnswerSurvayViewModel>(model);
                return new ResultDto<GetAnswerSurvayViewModel>
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            return new ResultDto<GetAnswerSurvayViewModel> { IsSuccess = false };
        }
        public ResultDto<List<EditFeature_dto>> FindAnswerKeyById(long Id)
        {
            var model = _context.SurveyAnswers.AsNoTracking().Where(p => p.SurveyId == Id && p.SurveyQuestionId==null).ToList();


            if (model != null)
            {
                var result = _mapper.Map<List<EditFeature_dto>>(model);
                return new ResultDto<List<EditFeature_dto>>
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            return new ResultDto<List<EditFeature_dto>> { IsSuccess = false };
        }
        //برای نمایش جواب های سوال در پنل ادمین
        public PagingDto<List<GetAnswerSurvayViewModel>> GetListPageineted(int page, int pageSize, long surveyQuestionId)
        {
            int rowCount = 0;
            var db = _context.SurveyAnswers.Where(p => p.Status == 1 && p.SurveyQuestionId == surveyQuestionId).AsNoTracking().AsQueryable();
            var paging = db.PagedResult(page, pageSize, out rowCount);
            var result = _mapper.ProjectTo<GetAnswerSurvayViewModel>(paging).ToList();
            return new PagingDto<List<GetAnswerSurvayViewModel>>
            {
                PageNo = page,
                PageSize = pageSize,
                TotalRecords = rowCount,
                Data = result
            };
        }

        public ResultDto Remove(long Id)
        {
            var model = _context.SurveyAnswers.Find(Id);
            model.Status = 0;
            _context.SurveyAnswers.Update(model);
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
    }
}
