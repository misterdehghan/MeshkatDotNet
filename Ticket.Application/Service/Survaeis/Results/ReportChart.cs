using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.WorkPlace;
using Azmoon.Application.Service.Survaeis.Results.Dto;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Azmoon.Application.Service.Survaeis.Results
{
  
   public interface IReportChart
    {
        ResultDto<ReportAmarNewDto> Latary(long survayId);
        ResultDto<List<ReportSurvayWorckPlaceDto>> ReportSurvayWorckPlaceDto(string survayKey);
        ResultDto<ReportSurvayDescDto> LataryDescriptions(long survayId);
        ResultDto<ReportAmarDto> LataryChart(long survayId);
    }
    public class ReportChart : IReportChart
    {
        private readonly IGetWorkPlace _getWorkPlace;
        private readonly IDataBaseContext _context;
        private readonly IGetWorkplacFirstToEndParent _workplacFirstToEndParent;
        public ReportChart(IDataBaseContext context,
            IGetWorkplacFirstToEndParent workplacFirstToEndParent,
            IGetWorkPlace getWorkPlace)
            {
            _context = context;
            _workplacFirstToEndParent = workplacFirstToEndParent;
            _getWorkPlace = getWorkPlace;
            }
        public ResultDto<ReportAmarNewDto> Latary(long survayId)
        {     //یافتن نظرسنجی
            var survay = _context.Surveys.AsNoTracking()
                .Where(p=>p.Id==survayId)
                .Include(p=>p.SurveyQuestions)
                .ThenInclude(p=>p.SurveyAnswers)
                .AsQueryable();
            if (survay.FirstOrDefault()!=null)
            {
                  //   استخراج جواب های کاربران در نظرسنجی
                var Resultsurvay = _context.SurvayResultAnswers.AsNoTracking()
                    .Where(p => p.SurveyId == survayId).ToList();
                if (Resultsurvay!=null && Resultsurvay.Count()>0)
                {

                    var model = new ReportAmarNewDto();
                    model.survayId = survayId;
                    model.survayTitle = survay.FirstOrDefault().Name;
                    var rrr = Resultsurvay.Select(p => p.SurveyAnswerQuestion.Split("|")
                    ).ToList();

                   //بدست آوردن شناسه سوال های نظر سنجی
                    var questionsIds= rrr.ElementAt(rrr.Count()-1)[0].Split(',').Where(p => p != "").Select(p =>
                             Int64.Parse(p)
                            )
                        .ToList();
                    // بدست آوردن عنوان سوال های نظرسنجی
                    List<string> _survayQuestionTitles = new List<string>();
                    foreach ( var question in questionsIds )
                        {
                       var name = _context.SurveyQuestions.Where(p =>p.Id==question).AsNoTracking().FirstOrDefault().Text;
                        _survayQuestionTitles.Add(name);
                        }
                    model.survayQuestionTitle = _survayQuestionTitles;
                    model.survayQuestionTitle.Add("IP");
                    model.survayQuestionTitle.Add("WorkPlaceUser");
                    //foreach (var item in rrr)
                    //{
                    //    var addlist = item[1].Split(',').Where(p => p != "")
                    //            .Select(p =>
                    //                Int64.Parse(p)
                    //            )
                    //            .ToList();
                    //    model.GetAnswers.Add(addlist);
                    //}
                    for (int i = 0; i < rrr.Count(); i++)
                    {
                        var addlist = rrr.ElementAt(i)[1].Split(',').Where(p => p != "")
                          .Select(p =>
                              (p)
                          )
                          .ToList();
                        addlist.Add(Resultsurvay.ElementAt(i).Ip);
                        addlist.Add(_workplacFirstToEndParent.FirstToEndParent((long)Resultsurvay.ElementAt(i).WorkPlaceSurveyId).Data);
                        model.GetAnswers.Add(addlist);
                        }
                    return new ResultDto<ReportAmarNewDto>()
                    {
                        Data=model,
                        IsSuccess = true
                    };

                }

            }


            return new ResultDto<ReportAmarNewDto>() { 
            IsSuccess=false
            };
        }
        public ResultDto<List<ReportSurvayWorckPlaceDto>> ReportSurvayWorckPlaceDto(string survayKey)
            {     //یافتن نظرسنجی
            var survay = _context.Surveys.AsNoTracking()
                .Where(p => p.UniqKey == survayKey)
                .FirstOrDefault();
            if (survay != null)
                {
                var survayResult = _context.SurvayResultAnswers.AsNoTracking().Where(p => p.SurveyId == survay.Id).ToList();
                var model = new List<ReportSurvayWorckPlaceDto>();
                //   استخراج جواب های کاربران در نظرسنجی
                var Resultsurvay = _context.SurvayResultAnswers.AsNoTracking()
                    .Where(p => p.SurveyId == survay.Id).ToList();
                if (Resultsurvay.Count>0)
                    {
                    var allWorckPlases = _getWorkPlace.Execute(null).Data;
                    for (int i = 0; i < allWorckPlases.Count(); i++)
                        {
                        var addlist = new ReportSurvayWorckPlaceDto();
                        addlist.Nafarat = survayResult.Where(p => (long)p.WorkPlaceSurveyId == allWorckPlases.ElementAt(i).Id).Count();
                        addlist.ParentId = allWorckPlases.ElementAt(i).ParentId;
                        addlist.Id = allWorckPlases.ElementAt(i).Id;
                        //  addlist.Add(Resultsurvay.ElementAt(i).Ip);
                        addlist.WorckPlaceTitle=(_workplacFirstToEndParent.FirstToEndParent(allWorckPlases.ElementAt(i).Id).Data);
                        model.Add(addlist);
                        }

                    return new ResultDto<List<ReportSurvayWorckPlaceDto>>()
                        {
                        IsSuccess = true,
                        Data = model,
                        Message = survay.Name
                        };
                    }

                }


            return new ResultDto<List<ReportSurvayWorckPlaceDto>>()
                {
                IsSuccess = false
                };
            }
        public ResultDto<ReportAmarDto> LataryChart(long survayId)
        {
            throw new NotImplementedException();
        }

        public ResultDto<ReportSurvayDescDto> LataryDescriptions(long survayId)
            {
            //یافتن نظرسنجی
            var survay = _context.Surveys.AsNoTracking()
                .Where(p => p.Id == survayId)
                .AsQueryable().FirstOrDefault();
            if (survay != null)
                {
                ReportSurvayDescDto model = new ReportSurvayDescDto();
         
                model.survayId = survayId;
                model.survayTitle = survay?.Name;
                //   استخراج جواب های کاربران در نظرسنجی
                var Resultsurvay = _context.SurveyAnswerDescriptives.AsNoTracking()
                    .Where(p => p.SurveyId == survayId && p.Text!=null).ToList();
                if (Resultsurvay.Count()>0)
                    {
                    List<ReportSurvayDescQuestionDto> LSt_Answeres = new List<ReportSurvayDescQuestionDto>();
                    foreach (var item in Resultsurvay)
                    {
                        if (!String.IsNullOrEmpty(item.Text))
                            {
                            var moq = new ReportSurvayDescQuestionDto();
                            moq.QuestionTitle = _context.SurveyQuestions.Where(p => p.Id == item.SurveyQuestionId).AsNoTracking().FirstOrDefault().Text;
                            moq.Text = item.Text;
                            moq.Ip = item.Ip;
                            var SurvayResultAnswers = _context.SurvayResultAnswers.AsNoTracking()
                            .Where(p => p.UserName == item.UserName && p.SurveyId== survayId).AsNoTracking().FirstOrDefault();
                            moq.WorkPlaceUser = SurvayResultAnswers != null? _workplacFirstToEndParent.FirstToEndParent((long)SurvayResultAnswers.WorkPlaceSurveyId).Data : "";
                            moq.QuestionId = item.SurveyQuestionId;
                            LSt_Answeres.Add(moq);
                            }
                      
                    }

                    model.GetAnseres = LSt_Answeres;
                    return new ResultDto<ReportSurvayDescDto>()
                        {
                        IsSuccess = true,
                        Data = model
                        };
                    }
                else
                    {
                    return new ResultDto<ReportSurvayDescDto>()
                        {
                        IsSuccess = false,
                        Data = model
                        };
                    }
                }
            return new ResultDto<ReportSurvayDescDto>()
                {
                IsSuccess = false
                };
           
            }
        }
}
