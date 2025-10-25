using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.Group;
using Azmoon.Application.Service.Survaeis.SurveyGroups;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.Surves;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Survayy
{
    public class CrudSurvayService : ICrudSurvayService
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;
        private readonly IGetChildrenGroup _getChildrenGroup;
        public CrudSurvayService(IDataBaseContext context, IMapper mapper, IGetChildrenGroup getChildrenGroup)
        {
            _context = context;
            _mapper = mapper;
            _getChildrenGroup = getChildrenGroup;
        }
        public ResultDto<AddSurvayDto> Add(AddSurvayDto dto, string username)
        {    var key= Guid.NewGuid().ToString();
            var user = _context.Users.Where(p => p.UserName == username).FirstOrDefault();
            var model = new Survey();
            model = _mapper.Map(dto, model);
            model.CreatorId = user.Id;
            model.UniqKey = key;
            _context.Surveys.Add(model);
          var result=  _context.SaveChanges();
            if (result>0)
            {
                foreach (var item in dto.Features)
                {
                    var sa = new SurveyAnswer();
                    sa = _mapper.Map(item, sa);
                    sa.SurveyId = model.Id;
                    _context.SurveyAnswers.Add(sa);
                }
                _context.SaveChanges();
                return new ResultDto<AddSurvayDto> { 
                    Data=_mapper.Map<AddSurvayDto>(model),
                    IsSuccess = true
                };
            }
            return new ResultDto<AddSurvayDto> { IsSuccess=false};
        }

        public ResultDto<AddSurvayDto> Edit(AddSurvayDto dto, string username)
        {
            var user = _context.Users.Where(p => p.UserName == username).FirstOrDefault();
            var model = _context.Surveys.Find(dto.Id);
            var key = Guid.NewGuid().ToString();
            model = _mapper.Map(dto, model);
            model.CreatorId = user.Id;
            if (String.IsNullOrEmpty(model.UniqKey))
            {
                model.UniqKey = key;
            }
            _context.Surveys.Update(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new ResultDto<AddSurvayDto>
                {
                    Data = _mapper.Map<AddSurvayDto>(model),
                    IsSuccess = true
                };
            }
            return new ResultDto<AddSurvayDto> { IsSuccess = false };
        }

        public ResultDto<GetSurvayDto> FindById(long Id)
        {
            var model = _context.Surveys.AsNoTracking().Where(p=>p.Id==Id).FirstOrDefault();
           
 
            if (model != null)
            {
                var result = _mapper.Map<GetSurvayDto>(model);
                return new ResultDto<GetSurvayDto>
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            return new ResultDto<GetSurvayDto> { IsSuccess = false };
        }

        public ResultDto<AddSurvayDto, List<EditFeature_dto>> FindByIdForEdit(long Id)
        {
            var model = _context.Surveys
                .AsNoTracking().Where(p => p.Id == Id).FirstOrDefault();


            if (model != null)
            {
                var result = _mapper.Map<AddSurvayDto>(model);
              
                var towResult = _context.SurveyAnswers.AsNoTracking()
                    .Where(p => p.SurveyId == Id && p.SurveyQuestion == null).Select(p => new EditFeature_dto { 
                Id=p.Id,
                Title= p.Title,
                Wight=p.Wight,
                SurveyId=p.SurveyId
                }).ToList();
                return new ResultDto<AddSurvayDto , List<EditFeature_dto> >
                {
                    Data = result,
                    TwoDate= towResult,
                    IsSuccess = true
                };
            }
            return new ResultDto<AddSurvayDto, List<EditFeature_dto>> { IsSuccess = false };
        }

        public PagingDto<List<GetSurvayDto>> GetListPageineted(int page, int pageSize , string q, int status, string userName)
        {
            int rowCount = 0;
            var user = _context.Users.Where(p => p.UserName == userName)
                          .Include(p => p.GroupUsers)
                          .FirstOrDefault(); 
            var db = _context.Surveys.Where(p=>p.Status==1).AsNoTracking()
                .Include(p=>p.Group).AsQueryable();
            var groupIdes = _getChildrenGroup.Exequte(user.GroupUsers.Select(p => p.GroupId).ToList()).Data;
            if (String.IsNullOrEmpty(q))
            {
                db = db.Where(p => p.Name.Contains(q)).AsQueryable();
            }
            if (user.GroupUsers != null && user.GroupUsers.Count() > 0)
            {
                db = db.Where(p => groupIdes.Contains(p.GroupId) || p.CreatorId == user.Id).AsQueryable();
            }
            else
            {
                db = db.Where(p => p.CreatorId == user.Id).AsQueryable();
            }
            var paging = db.PagedResult(page, pageSize, out rowCount);
            var result = _mapper.ProjectTo<GetSurvayDto>(paging).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                var start = (DateTime)result.ElementAt(i).StartDate;
                var startSuspand = start.AddHours(5.0);
                var end = (DateTime)result.ElementAt(i).EndDate;
                result.ElementAt(i).CountResoult = _context.SurvayResultAnswers.Where(p => p.SurveyId == result.ElementAt(i).Id).AsNoTracking().Count();
                if (start <= DateTime.Now && end > DateTime.Now)
                {
                    //در حال برگزاری
                    result.ElementAt(i).state = 1;
                }
                if (startSuspand >= DateTime.Now && start > DateTime.Now)
                {
                    //در انتظار شروع
                    result.ElementAt(i).state = 2;
                }
                if (end < DateTime.Now)
                {
                    //خاتمه یافته
                    result.ElementAt(i).state = 3;
                }
            }
            return new PagingDto<List<GetSurvayDto>> {
            PageNo= page,
            PageSize= pageSize,
            TotalRecords= rowCount,
            Data= result
            };
        }

        public ResultDto Remove(long Id)
        {
            var model = _context.Surveys.Find(Id);
            model.Status = 0;
            _context.Surveys.Update(model);
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
