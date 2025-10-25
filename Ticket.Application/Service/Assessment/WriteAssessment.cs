using Azmoon.Application.Interfaces.Assessment;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.Template;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment
    {
    public class WriteAssessment : IWriteAssessment
        {
        private readonly IDataBaseContext _context;

        public WriteAssessment(IDataBaseContext context)
            {
            _context = context;
            }

        public ResultDto<int> Add(AddTemplateDto dto, string username)
            {
            var user = _context.Users.Where(p => p.UserName == username).FirstOrDefault();
            if (user != null)
                {
                var template = new TemplateMain()
                    {
                    Id = 0,
                    CreatorId = user.Id,
                    Name = dto.Name,
                    RegesterAt = DateTime.Now,
                    Status = 1,
                    UpdatedAt = DateTime.Now,
                    };
                _context.TemplateMains.Add(template);
                var seved = _context.SaveChanges();
                if (seved > 0)
                    {
                    foreach (var item in dto.QuestionFeatures)
                        {
                        var qustion = new TemplateQuestionAnswer()
                            {
                            Id = 0,
                            QA_Type = 1,
                            Title = item.Title,
                            TemplateMainId = template.Id,
                            RegesterAt = DateTime.Now,
                            Status = 1,
                            UpdatedAt = DateTime.Now,
                            Wight = 0
                            };
                        _context.TemplateQuestionAnswers.Add(qustion);
                        var seve = _context.SaveChanges();
                        }
                    foreach (var item in dto.AnswerFeatures)
                        {
                        var answer = new TemplateQuestionAnswer()
                            {
                            Id = 0,
                            QA_Type = 2,
                            Title = item.Title,
                            TemplateMainId = template.Id,
                            RegesterAt = DateTime.Now,
                            Status = 1,
                            UpdatedAt = DateTime.Now,
                            Wight = item.Wight
                            };
                        _context.TemplateQuestionAnswers.Add(answer);
                        var seve = _context.SaveChanges();
                        }
                    return new ResultDto<int>()
                        {
                        Data = 0,
                        IsSuccess = true,
                        Message = "ثبت و ذخیره موفق"
                        };
                    }
                return new ResultDto<int>()
                    {
                    Data = 0,
                    IsSuccess = false,
                    Message = "مشکل در عملیات ذخره در دیتابیس"
                    };
                }

            return new ResultDto<int>()
                {
                Data = 0,
                IsSuccess = false,
                Message = "مشکل در ارسال اطلاعات"
                };
            }

        public ResultDto<int> AddAssessment(AddAssessmentDto dto, string username)
            {

         var json = JsonConvert.SerializeObject(dto.AddModaresFeatures);
         var jsonToListObj = JsonConvert.DeserializeObject(json );
            var jsonData = JsonConvert.DeserializeObject<List<AddModaresFeatureDto>>(json);
            foreach (var item in jsonData)
                {

                }
            var model = new Domain.Entities.Template.Assessment()
                {
                Id= dto.Id,
                CreatorUserName = username,
                Description = dto.Description,
                EndDate = (DateTime)dto.EndDate,
                StartDate = (DateTime)dto.StartDate,
                Name = dto.Name,
                TemplateMainId = dto.TemplateMainId,
                WorkPlaceId = dto.WorkPlaceId ,
                PeriodTeachers = json
                };
            if (model.Id > 0)
                {
                _context.Assessments.Update(model);
                }
            else
                {
                _context.Assessments.Add(model);
                }

            _context.SaveChanges();

            return new ResultDto<int>()
                {
                Data = model.Id,
                IsSuccess = true
                };
            }
     
        public ResultDto<int> Delete(int id)
            {
            var model = _context.TemplateMains.Where(p => p.Id == id).FirstOrDefault();
            if (model != null) {
                model.Status = 3;
                _context.SaveChanges();
                }
            return new ResultDto<int>() { 
                  IsSuccess=true
                };
            }
        public ResultDto<int> DeleteAssessment(int id)
            {
            var model = _context.Assessments.Where(p => p.Id == id).FirstOrDefault();
            if (model != null)
                {
                model.Status = 3;
                _context.SaveChanges();
                }
            return new ResultDto<int>()
                {
                IsSuccess = true
                };
            }

        public ResultDto<AddAssessmentDto> GetAssessment(int id, string username)
            {
            var dto = _context.Assessments.Where(p => p.Id == id).FirstOrDefault();
            if (dto != null)
                {
                var jsonObjModaresFeatures = JsonConvert.DeserializeObject<List<AddModaresFeatureDto>>(dto.PeriodTeachers);
                var model = new AddAssessmentDto
                    {
                    Id = dto.Id,
                    CreatorUserName = username,
                    Description = dto.Description,
                    EndDate = dto.EndDate,
                    StartDate = dto.StartDate,
                    Name = dto.Name,
                    TemplateMainId = dto.TemplateMainId,
                    WorkPlaceId = dto.WorkPlaceId,
                    WorkPlaceIdFake = _context.WorkPlaces.Where(p => p.Id == dto.WorkPlaceId).FirstOrDefault().Name,
                    AddModaresFeatures = jsonObjModaresFeatures
                    };

                return new ResultDto<AddAssessmentDto>()
                    {
                    Data = model,
                    IsSuccess = true
                    };
                }

            return new ResultDto<AddAssessmentDto>()
                {
                Data = null,
                IsSuccess = false
                };
            }
        }
    }
