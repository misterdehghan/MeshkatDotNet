using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.WorkPlace;
using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Application.Service.JameiatQustion.Dto;
using Azmoon.Common.ResultDto;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment
    {
    public interface IStarClientAssessment
        {
        GetStarClientAssessmentDto GetStarClientAssessment(int id);
        }
    public class StarClientAssessment : IStarClientAssessment
        {
        private readonly IDataBaseContext _context;

        public StarClientAssessment(IDataBaseContext context)
            {
            _context = context;

            }
        public GetStarClientAssessmentDto GetStarClientAssessment(int id)
            {
            GetStarClientAssessmentDto model = new GetStarClientAssessmentDto();
            var assessment = _context.Assessments.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
            if (assessment != null)
                {
                model.AssessmentId = assessment.Id;
                model.Name=assessment.Name;
                model.Description=assessment.Description;
                model.modaresFeatureDtos = JsonConvert.DeserializeObject<List<AddModaresFeatureDto>>(assessment.PeriodTeachers);
                var templat = _context.TemplateMains.AsNoTracking().Where(p => p.Id == assessment.TemplateMainId).FirstOrDefault();
           
                var result = new GetDitelesTemplateDto();
                result.Id = id;
                var qa = _context.TemplateQuestionAnswers.AsNoTracking().Where(p => p.TemplateMainId == templat.Id).ToList();
                result.Name = templat.Name;
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
                model.getQuestionAnswer = result;
                model.getJameiatQustions = _context.jameiatQustions.AsNoTracking().Select(p => new GetJameiatQustionViewModel() {
                         Id = p.Id,
                         Name = p.Name,
                         ParentId = p.ParentId,
                         typeQA = p.typeQA,
                         Wight = p.Wight
                    
                    }).ToList();

                return model;
                }
            return null;
            }
        }
    }
