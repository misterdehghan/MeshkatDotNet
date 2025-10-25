using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Survayy
{
    public interface IGetStartSurvay
    {
        ResultDto<GetStartSurvayViewModel> start(long survayId);
    }
    public class GetStartSurvay : IGetStartSurvay
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public GetStartSurvay(IDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ResultDto<GetStartSurvayViewModel> start(long survayId)
        {
            var nowDate = DateTime.Now;
            var survaydb = _context.Surveys.Where(p => p.Status == 1 &&
            p.Id == survayId &&
            p.StartDate <= nowDate &&
            p.EndDate >= nowDate
            ).AsNoTracking().FirstOrDefault();
            if (survaydb != null)
            {
                var model = new GetStartSurvayViewModel();
                var defultAnswer = _context.SurveyAnswers.Where(p => p.SurveyId == survayId && p.SurveyQuestionId == null).ToList();
                var questions = _context.SurveyQuestions.Where(
                    p => p.Status == 1 &&
                     p.SurveyId == survayId).OrderBy(p => p.QuestionType)
                    .Include(p => p.SurveyAnswers.Where(p=>p.Status==1))
                    .AsNoTracking().ToList();
                if (questions != null && questions.Count() > 0)
                {
                    List<GetQuestionSurvay> getQuestions = new List<GetQuestionSurvay>();
                    model.Description = survaydb.Description;
                    model.SurvayId = survaydb.Id;
                    model.Name = survaydb.Name;


                    foreach (var ques in questions)
                    {

                        var answeres = ques.SurveyAnswers;
                        if (answeres != null && answeres.Count() > 0)
                        {
                            getQuestions.Add(new GetQuestionSurvay() { 
                            Id= ques.Id,
                            Text=ques.Text,
                            QuestionType=ques.QuestionType,
                            getAnsweres=_mapper.Map<List<GetAnswerSurvayViewModel>>(answeres)
                            });
                        }
                        if (ques.QuestionType==0)
                        {
                            getQuestions.Add(new GetQuestionSurvay()
                            {
                                Id = ques.Id,
                                Text = ques.Text,
                                QuestionType = ques.QuestionType,
                                getAnsweres = defultAnswer.Select(p => new GetAnswerSurvayViewModel { 
                                Id=p.Id,
                                Index=p.Index,
                                SurveyId=p.SurveyId,
                                SurveyQuestionId= ques.Id,
                                Title=p.Title,
                                Wight=p.Wight
                                }).ToList()
                                //  getAnsweres = _mapper.Map<List<GetAnswerSurvayViewModel>>(defultAnswer)
                            }) ;
                        }
                        if (ques.QuestionType==2)
                        {
                            getQuestions.Add(new GetQuestionSurvay()
                            {
                                Id = ques.Id,
                                Text = ques.Text,
                                QuestionType = ques.QuestionType,
                                getAnsweres = null
                            });
                        }
                    }
                    model.getQuestion = getQuestions;

                    return new ResultDto<GetStartSurvayViewModel>
                    {
                        Data = model,
                        IsSuccess = true
                    };
                }
            }

            return new ResultDto<GetStartSurvayViewModel>
            {
                Data = null,
                IsSuccess = false
            };
        }
    }
}
