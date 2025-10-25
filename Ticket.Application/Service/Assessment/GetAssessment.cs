using Azmoon.Application.Interfaces.Assessment;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.WorkPlace;
using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Application.Service.JameiatQustion.Dto;
using Azmoon.Application.Service.User.Dto;
using Azmoon.Common.Pagination;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment
    {
    public class GetAssessment : IGetAssessment
        {
        private readonly IDataBaseContext _context;
        private readonly IGetChildrenWorkPlace _childrenWorkPlace;

        public GetAssessment(IDataBaseContext context, IGetChildrenWorkPlace childrenWorkPlace)
            {
            _context = context;
            _childrenWorkPlace = childrenWorkPlace;
            }

        public ResultDto<List<GetTemplatesDto>> GetTemplates(string username)
            {
            var user = _context.Users.Where(p => p.UserName == username).FirstOrDefault();
            if (user != null)
                {
                var model = _context.TemplateMains.Where(p=>p.Status!=3).Select(p => new GetTemplatesDto()
                    {
                    Id = p.Id,
                    Name = p.Name,
                    UserName = p.CreatorId,
                    }).ToList();
                if (model.Count > 0)
                    {
                    for (global::System.Int32 i = 0; i < model.Count; i++)
                        {

                        model.ElementAt(i).UserName = user.FirstName + " " + user.LastName;
                        }
                    return new ResultDto<List<GetTemplatesDto>>()
                        {
                        Data = model,
                        IsSuccess = true,
                        Message = "موفق"
                        };
                    }
                }

            return new ResultDto<List<GetTemplatesDto>>()
                {
                Data = null,
                IsSuccess = false,
                Message = "موردی یافت نگردید"
                };
            }

        public ResultDto<GetDitelesTemplateDto> GetTemplateQustionAnswers(int id, string username)
            {
            var model = _context.TemplateMains.Where(p => p.Id == id).FirstOrDefault();
            if (model != null)
                {
                var result = new GetDitelesTemplateDto();
                result.Id = id;
                var qa = _context.TemplateQuestionAnswers.Where(p => p.TemplateMainId == model.Id).ToList();
                result.Name = model.Name;
                result.QuestionFeatures = qa.Where(p => p.QA_Type == 1).Select(p => new AddQustionFeatureDto()
                    {
                    Id = p.Id,
                    Title = p.Title

                    }).ToList();
                result.AnswerFeatures = qa.Where(p => p.QA_Type == 2).Select(p => new AddAnswerFeatureDto()
                    {
                    Id = p.Id,
                    Title = p.Title,
                    Wight = p.Wight

                    }).ToList();
                return new ResultDto<GetDitelesTemplateDto>()
                    {
                    Data = result,
                    IsSuccess = true,
                    Message = "موفق "
                    };
                }
            return new ResultDto<GetDitelesTemplateDto>()
                {
                Data = null,
                IsSuccess = false,
                Message = "خطا در ارسال اطلاعات"
                };
            }
        public ResultDto<GetDitelesTemplateDto> GetViewTemplateQustionAnswers(int id, string username)
            {
            var assessment = _context.Assessments.Where(p => p.Id == id).FirstOrDefault();
            var model = _context.TemplateMains.Where(p => p.Id == assessment.TemplateMainId).FirstOrDefault();
            if (model != null)
                {
                var jsonData = JsonConvert.DeserializeObject<List<AddModaresFeatureDto>>(assessment.PeriodTeachers);
                var result = new GetDitelesTemplateDto();
                result.Id = id;
                var qa = _context.TemplateQuestionAnswers.Where(p => p.TemplateMainId == model.Id).ToList();
                var jameiatQuestions = _context.jameiatQustions.AsNoTracking().Where(p => p.Status == 1).Select(p=> new GetJameiatQustionViewModel { 
                     Id = p.Id,
                     Name = p.Name,
                     typeQA=p.typeQA,
                     Wight= p.Wight ,
                     ParentId=p.ParentId
                    }).ToList();
                result.Name = model.Name;
                result.QuestionFeatures = qa.Where(p => p.QA_Type == 1).Select(p => new AddQustionFeatureDto()
                    {
                    Id = p.Id,
                    Title = p.Title

                    }).ToList();
                result.AnswerFeatures = qa.Where(p => p.QA_Type == 2).Select(p => new AddAnswerFeatureDto()
                    {
                    Id = p.Id,
                    Title = p.Title,
                    Wight = p.Wight

                    }).ToList();
                result.JameiatQustions = jameiatQuestions;
                result.ModaresFeatures = jsonData;
                return new ResultDto<GetDitelesTemplateDto>()
                    {
                    Data = result,
                    IsSuccess = true,
                    Message = "موفق "
                    };
                }
            return new ResultDto<GetDitelesTemplateDto>()
                {
                Data = null,
                IsSuccess = false,
                Message = "خطا در ارسال اطلاعات"
                };
            }
        public ResultDto<GetAssessmentPagination> GetAssessmentPagination(string username, int page, int PageSize, bool userIsAdmin = false, int templateId = 0, long worckplae = 0, string q = "" )
            {
            int rowCount = 0;
            var user = _context.Users.Where(p => p.UserName == username).Include(p=>p.UserRoles).FirstOrDefault();
            if (user != null)
                {
                var userAccess = _context.UserAccess.Where(p => p.UserName == username).FirstOrDefault();
                if (userAccess != null)
                    {
                    long wpSearch = userAccess.WorkPlaceId;
                    if (userIsAdmin)
                        {
                        wpSearch = 1;
                        }
                  
                    var worckplaces = _childrenWorkPlace.ExequteById(wpSearch);
                    var model = _context.Assessments.Where(p => worckplaces.Data.Contains((long)p.WorkPlaceId) && p.Status<3).AsQueryable();
                    if (!String.IsNullOrEmpty(q))
                        {
                        model = model.Where(p => p.Name.Contains(q.Trim())).AsQueryable();
                        }
                    if (worckplae != 0)
                        {
                        model = model.Where(p => p.WorkPlaceId == worckplae).AsQueryable();
                        }
                    if (templateId != 0)
                        {
                        model = model.Where(p => p.TemplateMainId == templateId).AsQueryable();
                        }
                    var paging = model.ToPaged(page, PageSize, out rowCount).ToList();

                    var result = model.Select(p => new GetAssessmentDto()
                        {
                        Id = p.Id,
                        Name = p.Name,
                        CreatorUserName = _context.Users.Where(l => l.UserName == p.CreatorUserName).Select(l => new
                            {
                            l.FirstName,
                            l.LastName
                            }).FirstOrDefault().ToString().Remove(0, 14).Replace("LastName =", "").Replace("}", ""),
                        TemplateMainName = _context.TemplateMains.Where(k => k.Id == p.TemplateMainId).FirstOrDefault().Name,
                        WorkPlaceName = _context.WorkPlaces.Where(o => o.Id == p.WorkPlaceId).FirstOrDefault().Name,
                        EndDate = p.EndDate,
                        StartDate = p.StartDate,
                        Description = p.Description,
                        Status = p.Status,
                        countUser = _context.TemplateUserResultAnswers.Where(k => k.AssessmentId == p.Id).Count(),
                        }).ToList();
                    var modell = new GetAssessmentPagination();
                    modell.PagerDto = new PagerDto
                        {
                        PageNo = page,
                        PageSize = PageSize,
                        TotalRecords = rowCount
                        };
                    modell.Assessmentes = result;
                    return new ResultDto<GetAssessmentPagination>()
                        {
                        Data = modell,
                        IsSuccess = true
                        };
                    }

                }
            return new ResultDto<GetAssessmentPagination>() { IsSuccess = false };
            }

        public ResultDto<int> EditTemplateQustionAnswers(EditTemplateQustionAnswersDto dto, string userName , string Name)
            {
            var user = _context.Users.Where(p => p.UserName == userName).FirstOrDefault();
            var templatee = _context.TemplateMains.Where(p => p.Id == dto.TemplateId).FirstOrDefault();
            if (user != null && templatee!=null)
                {
                templatee.Name = Name;
                for (int i = 0; i < dto.QuestionFeaturTitle.Count(); i++)
                    {

                    string hk = dto.QuestionFeaturTitle.ElementAt(i).Key;
                    string hv = dto.QuestionFeaturTitle.ElementAt(i).Value;
                    int key = Int32.Parse(dto.QuestionFeaturTitle.ElementAt(i).Key);
                    var qa = _context.TemplateQuestionAnswers.Where(p => p.TemplateMainId == dto.TemplateId && p.Id == key).FirstOrDefault();
                    if (qa != null)
                        {
                        qa.Title = dto.QuestionFeaturTitle.ElementAt(i).Value;
                        _context.SaveChanges();
                        }

                    }
                for (global::System.Int32 i = 0; i < dto.answerFeaturTitle.Count(); i++)
                    {

                    int key = Int32.Parse(dto.answerFeaturTitle.ElementAt(i).Key);
                    var qa = _context.TemplateQuestionAnswers.Where(p => p.TemplateMainId == dto.TemplateId && p.Id == key).FirstOrDefault();
                    if (qa != null)
                        {
                        qa.Title = dto.answerFeaturTitle.ElementAt(i).Value;
                        qa.Wight = Int32.Parse(dto.answerFeaturWight.ElementAt(i).Value);
                        _context.SaveChanges();
                        }
                    }

                }
            return new ResultDto<int> { IsSuccess = true };
            }

        public ResultDto<List<GetAssessmentDto>> GetAssessmentWpId(long wpId)
            {
            var model = _context.Assessments.AsNoTracking().Where(p => p.WorkPlaceId == wpId && p.EndDate >= DateTime.Now && p.StartDate <= DateTime.Now && p.Status != 3)
                   .Select(p => new GetAssessmentDto()
                       {
                       Id = p.Id,
                       Name = p.Name,
                       WorkPlaceName = _context.WorkPlaces.Where(o => o.Id == p.WorkPlaceId).FirstOrDefault().Name,
                       EndDate = p.EndDate,
                       StartDate = p.StartDate,
                       Description = p.Description,
                       Status = p.Status,
                       }).ToList();
            return new ResultDto<List<GetAssessmentDto>>()
                {
                Data = model,
                IsSuccess = model.Count > 0 ? true : false
                };
            }

        public ResultDto<GetAssessmentPagination> GetSearchAssessment(string username, string qSearch, DateTime? StartDate, DateTime? EndDate, long WorkPlaceId = 0)
            {
            int rowCount = 0;
            var user = _context.Users.Where(p => p.UserName == username).FirstOrDefault();
            if (user != null)
                {
                var userAccess = _context.UserAccess.Where(p => p.UserName == username).FirstOrDefault();
                if (userAccess != null)
                    {

                    var model = _context.Assessments.AsQueryable();
                    if (!String.IsNullOrEmpty(qSearch))
                        {
                        model = model.Where(p => p.Name.Contains(qSearch.Trim())).AsQueryable();
                        }
                    if (WorkPlaceId != 0)
                        {
                        var worckplaces = _childrenWorkPlace.ExequteById(userAccess.WorkPlaceId);
                        model = model.Where(p => worckplaces.Data.Contains((long)p.WorkPlaceId)).AsQueryable();
                        }
                    if (StartDate != null)
                        {
                        model = model.Where(p => p.StartDate >= StartDate).AsQueryable();
                        }
                    if (EndDate != null)
                        {
                        model = model.Where(p => p.EndDate <= EndDate).AsQueryable();
                        }
                    var paging = model.ToPaged(1, 100, out rowCount).ToList();

                    var result = model.Select(p => new GetAssessmentDto()
                        {
                        Id = p.Id,
                        Name = p.Name,
                        CreatorUserName = _context.Users.Where(l => l.UserName == p.CreatorUserName).Select(l => new
                            {
                            l.FirstName,
                            l.LastName
                            }).FirstOrDefault().ToString().Remove(0, 14).Replace("LastName =", "").Replace("}", ""),
                        TemplateMainName = _context.TemplateMains.Where(k => k.Id == p.TemplateMainId).FirstOrDefault().Name,
                        WorkPlaceName = _context.WorkPlaces.Where(o => o.Id == p.WorkPlaceId).FirstOrDefault().Name,
                        EndDate = p.EndDate,
                        StartDate = p.StartDate,
                        Description = p.Description,
                        Status = p.Status,
                        countUser = _context.TemplateUserResultAnswers.Where(k => k.AssessmentId == p.Id).Count(),
                        }).ToList();
                    var modell = new GetAssessmentPagination();
                    modell.PagerDto = new PagerDto
                        {
                        PageNo = 1,
                        PageSize = 100,
                        TotalRecords = rowCount
                        };
                    modell.Assessmentes = result;
                    return new ResultDto<GetAssessmentPagination>()
                        {
                        Data = modell,
                        IsSuccess = true
                        };
                    }

                }
            return new ResultDto<GetAssessmentPagination>() { IsSuccess = false };
            }


        public class GetSearchAssessmentDto
            {

            public int WorkPlaceId { get; set; }
            public string qSearch { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string WorkPlaceIdFake { get; set; }
            }
        }
    }
