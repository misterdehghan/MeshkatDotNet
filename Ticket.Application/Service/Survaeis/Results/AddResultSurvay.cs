using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.Surves;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Results
{
    public interface IAddResultSurvay
    {
        ResultDto addResultSurvay(DataResultSurvayDto dto);
    }
    public class AddResultSurvay : IAddResultSurvay
    {
        private readonly IDataBaseContext _context;

        public AddResultSurvay(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto addResultSurvay(DataResultSurvayDto dto)
        {
            var survay = _context.Surveys
                     .Where(p => p.Id == dto.SurvayId)
                     .AsNoTracking()
                     .FirstOrDefault();
            if (survay != null)
            {
                var resultCount = _context.SurvayResultAnswers.Where(p => p.SurveyId == dto.SurvayId && p.Ip == dto.Ip).AsNoTracking().ToList();
                if (resultCount != null && resultCount.Count() >= survay.AllowCountPerIp)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "تعداد درخواست شما برای شرکت در نظر سنجی مجاز نمی باشد!!!!"
                    };
                }

                if (dto.answer != null && dto.answer.Count() > 0)
                {
                    var questiones = dto.answer.Select(p => new { key = Int64.Parse(p.Key) }).ToArray();
                    var answeres = dto.answer.Select(p => new { value = p.Value }).ToArray();

                    string answeresIdSurvay = "";
                    string answerWightSurvay = "";
                    string questionesInSurvay = "";
                    string typeQuestionSurvay = "";


                    for (int i = 0; i < questiones.Length; i++)
                    {
                        var answereSplit = answeres[i].value.Split('_');
                        var ClientAnswerId =Int64.Parse(answereSplit[0]) ;
                        var ClientAnswerWight = Int64.Parse(answereSplit[1]) ;
                        var questioneskey = questiones[i].key;
                        var answer = _context.SurveyAnswers
                                                    .Where(p => p.Id == ClientAnswerId )
                                                    .AsNoTracking()
                                                    .FirstOrDefault();
                        var question = _context.SurveyQuestions
                                                   .Where(p => p.Id == questioneskey )
                                                   .AsNoTracking()
                                                   .FirstOrDefault();
                        if (answer != null && question!=null)
                        {

                            answeresIdSurvay = answeresIdSurvay + ClientAnswerId + ",";
                            answerWightSurvay = answerWightSurvay + ClientAnswerWight + ",";
                            questionesInSurvay = questionesInSurvay + questiones[i].key + ",";
                            typeQuestionSurvay= typeQuestionSurvay + question.QuestionType+ ",";
                        }
                    }

                    var resultsurvay = new SurvayResultAnswer() { 
                    Id=0,
                    Ip=dto.Ip,
                    CreateAt=DateTime.Now,
                    SurveyAnswerQuestion= questionesInSurvay+"|"+ answerWightSurvay+"|"+ answeresIdSurvay+"|"+typeQuestionSurvay,
                    SurveyId=dto.SurvayId,
                    UserName=dto.username,
                    WorkPlaceSurveyId=dto.WorkPlaceId
                    };
                    _context.SurvayResultAnswers.Add(resultsurvay);
                   var rr = _context.SaveChanges();
                    if (rr<1)
                    {
                        return new ResultDto
                        {
                            IsSuccess = false,
                            Message = "خطا در دریافت اطلاعات سمت سرور!!!!"
                        };
                    }

                }
                if (dto.deccriptionAnswers != null && dto.deccriptionAnswers.Count() > 0)
                {
                    var decquestiones = dto.deccriptionAnswers.Select(p => new { key = Int64.Parse(p.Key) }).ToArray();
                    var decansweres = dto.deccriptionAnswers.Select(p => new { value = p.Value }).ToArray();
                    for (int i = 0; i < decquestiones.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(decansweres[i].value))
                            {
                            var dec = new SurveyAnswerDescriptive()
                                {
                                Id = 0,
                                Ip = dto.Ip,
                                RegesterAt = DateTime.Now,
                                SurveyId = dto.SurvayId,
                                SurveyQuestionId = decquestiones[i].key,
                                Text = decansweres[i].value,
                                UserName = dto.username
                                };
                            _context.SurveyAnswerDescriptives.Add(dec);
                            var bbb = _context.SaveChanges();
                            if (bbb < 1)
                                {
                                return new ResultDto
                                    {
                                    IsSuccess = false,
                                    Message = "خطا در دریافت اطلاعات سمت سرور!!!!"
                                    };
                                }
                            }
             
                   
                    }

                }


                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "با تشکر !! نظرسنجی با موفقیت ثبت گردید."
                };


            }


            return new ResultDto {
                IsSuccess = false,
                Message ="اشکال در ارسال اطلاعات!!!"
            };
        }
    }
}
