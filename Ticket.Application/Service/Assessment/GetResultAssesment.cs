using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Application.Service.Survaeis.Results.Dto;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Common.ResultDto;
using Azmoon.Common.Useful;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Azmoon.Application.Service.Assessment
    {
    public interface IGetResultAssesment
        {
        ResultDto<ReportAmarNewDto> Latary(int assessmentId);
        ResultDto<GetResultAssessmentDescription> AssessmentDescriptions(int assessmentId);
        }
    public class GetResultAssesment : IGetResultAssesment
        {
        private readonly IDataBaseContext _context;
        private readonly IGetWorkplacFirstToEndParent _workplacFirstToEndParent;
        public GetResultAssesment(IDataBaseContext context, IGetWorkplacFirstToEndParent workplacFirstToEndParent)
            {
            _context = context;
            _workplacFirstToEndParent = workplacFirstToEndParent;
            }
        public ResultDto<ReportAmarNewDto> Latary(int assessmentId)
            {     //یافتن نظرسنجی
            var survay = _context.Assessments.AsNoTracking()
                .Where(p => p.Id == assessmentId)
                .AsQueryable();
            if (survay.FirstOrDefault() != null)
                {
                var jsonDataModaresFeature = JsonConvert.DeserializeObject<List<AddModaresFeatureDto>>(survay.FirstOrDefault().PeriodTeachers);
                //   استخراج جواب های کاربران در نظرسنجی
                var Resultsurvay = _context.TemplateUserResultAnswers.AsNoTracking()
                    .Where(p => p.AssessmentId == assessmentId).ToList();
                if (Resultsurvay != null && Resultsurvay.Count() > 0)
                    {

                    var model = new ReportAmarNewDto();
                    model.survayId = assessmentId;
                    model.survayTitle = survay.FirstOrDefault().Name;
                    var rrr = Resultsurvay.Select(p => p.TemplateAnswerQuestion.Split("|")
                    ).ToList();
                    var JameiatAnswer = Resultsurvay.Select(p => p.JaneiatAnswerQuestion.Split("|")
                 ).ToList();
                    var modaresJavabs = Resultsurvay.Select(p => p.ModaresAnswer.Split("|")
             ).ToList();
                    //بدست آوردن شناسه سوال های نظر سنجی
                    List<long> questionsIds = new List<long>();
                     questionsIds = rrr.ElementAt(rrr.Count() - 1)[0].Split(',').Where(p => p != "").Select(p =>
                             Int64.Parse(p)
                            )
                        .ToList();
                   var questionsIds2 = JameiatAnswer.ElementAt(rrr.Count() - 1)[0].Split(',').Where(p => p != "").Select(p =>
                         Int64.Parse(p)
                        )
                    .ToList();
                    // بدست آوردن عنوان سوال های نظرسنجی
                    List<string> _survayQuestionTitles = new List<string>();
                    foreach (var question in questionsIds)
                        {
                        var name = _context.TemplateQuestionAnswers.Where(p => p.Id == question).AsNoTracking().FirstOrDefault().Title;
                        _survayQuestionTitles.Add(name);
                        }
                    foreach (var question in questionsIds2)
                        {
                        var name = _context.jameiatQustions.Where(p => p.Id == question).AsNoTracking().FirstOrDefault().Name;
                        _survayQuestionTitles.Add(name);
                        }
                    foreach (var item in jsonDataModaresFeature)
                    {
                        _survayQuestionTitles.Add(item.Modares+" درس : "+item.Title);
                     }
                    model.survayQuestionTitle = _survayQuestionTitles;
                    model.survayQuestionTitle.Add("IP");
                    model.survayQuestionTitle.Add("WorkPlaceUser");
            
                    for (int i = 0; i < rrr.Count(); i++)
                        {
                        var addlist = rrr.ElementAt(i)[1].Split(',').Where(p => p != "")
                          .Select(p =>
                              (p)
                          )
                          .ToList();
                        addlist.AddRange (JameiatAnswer.ElementAt(i)[1].Split(',').Where(p => p != "")
                          .Select(p =>
                              (p)
                          )
                          .ToList());

                        addlist.AddRange(modaresJavabs.ElementAt(i)[1].Split(',').Where(p => p != "")
                      .Select(p =>
                          (p)
                      )
                      .ToList());
                        addlist.Add(Resultsurvay.ElementAt(i).Ip);
                        addlist.Add(_workplacFirstToEndParent.FirstToEndParent((long)Resultsurvay.ElementAt(i).WorkPlaceId).Data);
                        model.GetAnswers.Add(addlist);
                        }
                    return new ResultDto<ReportAmarNewDto>()
                        {
                        Data = model,
                        IsSuccess = true
                        };

                    }

                }


            return new ResultDto<ReportAmarNewDto>()
                {
                IsSuccess = false
                };
            }

        public ResultDto<GetResultAssessmentDescription> AssessmentDescriptions(int assessmentId)
            {
            //یافتن نظرسنجی
            var assessments = _context.Assessments.AsNoTracking()
                .Where(p => p.Id == assessmentId)
                .AsQueryable().FirstOrDefault();
            if (assessments != null)
                {
                GetResultAssessmentDescription model = new GetResultAssessmentDescription();

                model.AssessmentId = assessmentId;
                model.AssessmentTitle = assessments?.Name;
                //   استخراج جواب های کاربران در نظرسنجی
                var Resultsurvay = _context.TemplateDescriptiveAnswers.AsNoTracking()
                    .Where(p => p.AssessmentId == assessmentId && p.Description != null).ToList();
                if (Resultsurvay.Count() > 0)
                    {
                    List<ReportDescAssessmenDto> LSt_Answeres = new List<ReportDescAssessmenDto>();
                    foreach (var item in Resultsurvay)
                        {
                        if (!String.IsNullOrEmpty(item.Description))
                            {
                            var moq = new ReportDescAssessmenDto();
                            moq.Text = item.Description;
                            moq.Ip = item.Ip;
                            var assessmentResultAnswers = _context.TemplateUserResultAnswers.AsNoTracking()
                            .Where(p => p.UserName == item.UserName && p.AssessmentId == assessmentId).AsNoTracking().FirstOrDefault();
                            moq.WorkPlaceUser = assessmentResultAnswers != null ? _workplacFirstToEndParent.FirstToEndParent((long)assessmentResultAnswers.WorkPlaceId).Data : "";
                            moq.datetime = item.CreateAt.ToPersianDateFullTime().Replace(" ","_");
                            LSt_Answeres.Add(moq);
                            }

                        }

                    model.GetAnseres = LSt_Answeres;
                    return new ResultDto<GetResultAssessmentDescription>()
                        {
                        IsSuccess = true,
                        Data = model
                        };
                    }
                else
                    {
                    return new ResultDto<GetResultAssessmentDescription>()
                        {
                        IsSuccess = false,
                        Data = model
                        };
                    }
                }
            return new ResultDto<GetResultAssessmentDescription>()
                {
                IsSuccess = false
                };

            }
        }
    }
