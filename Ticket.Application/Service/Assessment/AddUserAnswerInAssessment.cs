using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities;
using Azmoon.Domain.Entities.Surves;
using Azmoon.Domain.Entities.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment
    {
    public interface IAddUserAnswerInAssessment
        {
        ResultDto<int> Add(UserAnswerInAssessmentDto dto);
        }
    public class AddUserAnswerInAssessment : IAddUserAnswerInAssessment
        {
        private readonly IDataBaseContext _context;

        public AddUserAnswerInAssessment(IDataBaseContext context)
            {
            _context = context;
            }

        public ResultDto<int> Add(UserAnswerInAssessmentDto dto)
            {
               var assessment = _context.Assessments.AsNoTracking().FirstOrDefault(p=>p.Id==dto.AssessmentId);
            var resultCount = _context.TemplateUserResultAnswers.AsNoTracking().Where(p => p.AssessmentId == dto.AssessmentId && p.Ip == dto.Ip).ToList();
            if (resultCount != null && resultCount.Count() >= assessment.AllowCountPerIp)
                {
                return new ResultDto<int>
                    {
                    IsSuccess = false,
                    Message = "تعداد درخواست شما برای شرکت در نظر سنجی مجاز نمی باشد!!!!"
                    };
                }
            if (dto.answer != null && dto.answer.Count() > 0)
                {
                var questionesChandgozineh = dto.answer.Select(p => new { key = Int64.Parse(p.Key) }).ToArray();
                var answeresChandgozineh = dto.answer.Select(p => new { value = p.Value }).ToArray();
                var questionesJameiat = dto.jameiatanswer.Select(p => new { key = Int64.Parse(p.Key) }).ToArray();
                var answeresJameiat = dto.jameiatanswer.Select(p => new { value = p.Value }).ToArray();


                var questionesModares = dto.modaresAnswers.Select(p => new { key = Int64.Parse(p.Key) }).ToArray();
                var answeresModares = dto.modaresAnswers.Select(p => new { value = p.Value }).ToArray();


                string answeresIdChandgozineh = "";
                string answerWightChandgozineh = "";
                string questionesIdChandgozineh = "";

                string answeresIdJameiat = "";
                string answerWightJameiat = "";
                string questionesIdJameiat = "";


                string answeresIdModres = "";
                string answerWightModres = "";
                string questionesIdModres = "";


                for (int i = 0; i < questionesChandgozineh.Length; i++)
                    {
                    var answereSplit = answeresChandgozineh[i].value.Split('_');
                    var ClientAnswerId = Int64.Parse(answereSplit[0]);
                    var ClientAnswerWight = Int64.Parse(answereSplit[1]);
                    var questioneskey = questionesChandgozineh[i].key;
                    var answer = _context.TemplateQuestionAnswers
                                                .Where(p => p.Id == ClientAnswerId && p.QA_Type==2)
                                                .AsNoTracking()
                                                .FirstOrDefault();
                    var question = _context.TemplateQuestionAnswers
                                               .Where(p => p.Id == questioneskey && p.QA_Type == 1)
                                               .AsNoTracking()
                                               .FirstOrDefault();
                    if (answer != null && question != null)
                        {

                        answeresIdChandgozineh = answeresIdChandgozineh + ClientAnswerId + ",";
                        answerWightChandgozineh = answerWightChandgozineh + ClientAnswerWight + ",";
                        questionesIdChandgozineh = questionesIdChandgozineh + questionesChandgozineh[i].key + ",";
                       
                        }
                    }

                for (int i = 0; i < questionesJameiat.Length; i++)
                    {
                    var answereSplit = answeresJameiat[i].value.Split('_');
                    var ClientAnswerId = Int64.Parse(answereSplit[0]);
                    var ClientAnswerWight = Int64.Parse(answereSplit[1]);
                    var questioneskey = questionesJameiat[i].key;
                    var answer = _context.jameiatQustions
                                                .Where(p => p.Id == ClientAnswerId && p.ParentId != null)
                                                .AsNoTracking()
                                                .FirstOrDefault();
                    var question = _context.jameiatQustions
                                               .Where(p => p.Id == questioneskey && p.ParentId == null)
                                               .AsNoTracking()
                                               .FirstOrDefault();
                    if (answer != null && question != null)
                        {

                        answeresIdJameiat = answeresIdJameiat + ClientAnswerId + ",";
                        answerWightJameiat = answerWightJameiat + ClientAnswerWight + ",";
                        questionesIdJameiat = questionesIdJameiat + questionesJameiat[i].key + ",";

                        }
                    }
                for (int i = 0; i < questionesModares.Length; i++)
                    {
                    var answereSplit = answeresModares[i].value.Split('_');
                    var ClientAnswerId = Int64.Parse(answereSplit[0]);
                    var ClientAnswerWight = Int64.Parse(answereSplit[1]);
                    answeresIdModres =     answeresIdModres   + ClientAnswerId + ",";
                    answerWightModres  =   answerWightModres  + ClientAnswerWight + ",";
                    questionesIdModres =   questionesIdModres + questionesModares[i].key + ",";

                    }


                    var resultAssessment = new TemplateUserResultAnswer()
                    {
                    Id = 0,
                    Ip = dto.Ip,
                    CreateAt = DateTime.Now,
                    TemplateMainId = assessment.TemplateMainId,
                    TemplateAnswerQuestion = questionesIdChandgozineh + "|" + answerWightChandgozineh + "|" + answeresIdChandgozineh,
                    JaneiatAnswerQuestion  = questionesIdJameiat + "|" + answerWightJameiat + "|" + answeresIdJameiat,
                    ModaresAnswer = questionesIdModres + "|" + answerWightModres + "|" + answeresIdModres,
                    AssessmentId = dto.AssessmentId,
                    UserName = dto.username,
                    WorkPlaceId = dto.WorkPlaceId
                    };
                _context.TemplateUserResultAnswers.Add(resultAssessment);
                var rr = _context.SaveChanges();
                if (rr < 1)
                    {
                    return new ResultDto <int>
                        {
                        IsSuccess = false,
                        Message = "خطا در دریافت اطلاعات سمت سرور!!!!"
                        };
                    }
                if (dto.deccriptionAnswers != null && dto.deccriptionAnswers!="")
                    {
                            var dec = new TemplateDescriptiveAnswer()
                                {
                                Id = 0,
                                Ip = dto.Ip,
                                CreateAt = DateTime.Now,
                                TemplateMainId=assessment.TemplateMainId,
                                AssessmentId=dto.AssessmentId,
                                WorkPlaceId=dto.WorkPlaceId,
                                Description = dto.deccriptionAnswers,
                                UserName = dto.username
                                };
                            _context.TemplateDescriptiveAnswers.Add(dec);
                            var bbb = _context.SaveChanges();
                            if (bbb < 1)
                                {
                                return new ResultDto<int>
                                    {
                                    IsSuccess = false,
                                    Message = "خطا در دریافت اطلاعات سمت سرور!!!!"
                                    };
                                }
                       

                    }
                return new ResultDto <int>
                    {
                    IsSuccess = true,
                    Message = "با تشکر !! نظرسنجی با موفقیت ثبت گردید."
                    };
                }
            return new ResultDto<int>
                {
                IsSuccess = false,
                Message = "اشکال در ارسال اطلاعات!!!"
                };
            }
        }
    }
