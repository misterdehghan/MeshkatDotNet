using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.Result;
using Azmoon.Application.Service.Result.Dto;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmoon.Common.Useful;
using Azmoon.Domain.Entities;
using Azmoon.Application.Service.Question.Dto;
using Azmoon.Application.Service.QuizTemp.Dto;

namespace Azmoon.Application.Service.Result.Cammand
    {
    public class AddResultQuiz : IAddResultQuiz
        {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public AddResultQuiz(IDataBaseContext context, IMapper mapper)
            {
            _context = context;
            _mapper = mapper;
            }
        public ResultDto addResultQuizAdmin(string username, long quizId, int questionCounter, int trueQuestion, int falseQuestion, DateTime date, string ip)
            {
            var quiz = _context.Quizzes
                  .Where(p => p.Id == quizId)
                  .AsNoTracking()
                  .FirstOrDefault();
            var studentId = _context.Users
                               .Where(p => p.UserName == username)
                               .AsNoTracking()
                               .FirstOrDefault().Id;

            var quizStartTemps = _context.QuizStartTemps.Where(p => p.QuizId == quizId && p.UserName == username && p.RegesterAt > quiz.StartDate).FirstOrDefault();
            if (quizStartTemps == null || quizStartTemps.EndDate.AddMinutes(1) < DateTime.Now)
                {
                QuizStartTemp quiztemp = new QuizStartTemp()
                    {
                    Ip = ip,
                    QuizId = quizId,
                    StartDate = date.AddMinutes(-20),
                    EndDate = date,
                    UserName = username,
                    };
                _context.QuizStartTemps.Add(quiztemp);
                _context.SaveChanges();
                }
            else
                {
                quizStartTemps.StartDate = date.AddMinutes(-20);
                quizStartTemps.EndDate = date;
                _context.QuizStartTemps.Update(quizStartTemps);
                _context.SaveChanges();
                }
            List<AttemtedQuizQuestionViewModel> qustiones = new List<AttemtedQuizQuestionViewModel>();
            var Question = _context.Qestions
             .Where(p => p.QuizId == quizId && p.Status == 1)
             .AsNoTracking()
             .ToList();
            var lstIndexQuestion = getArreyIndex(Question.Count, questionCounter);
            foreach (var item in lstIndexQuestion)
                {
                var ques = new AttemtedQuizQuestionViewModel
                    {
                    Id = Question[item].Id,
                    Text = Question[item].Text,
                    Answers = GetAnswerForQuestionId(Question[item].Id)
                    };
                qustiones.Add(ques);
                }
            string answeresInQuiz = "";
            int TrueAnswer = 0;
            int FalseAnswer = 0;


            for (int i = 0; i < questionCounter; i++)
                {
                var question = qustiones.ElementAt(i);
                var answers = question.Answers;
                if (TrueAnswer < trueQuestion)
                    {



                    var answer = answers.FirstOrDefault(p => p.IsTrue == true);
                    answeresInQuiz = answeresInQuiz + question.Id + ":" + answer.Id + "|";
                    TrueAnswer++;
                    }
                else
                    {
                    var answer = answers.FirstOrDefault(p => p.IsTrue == false);
                    answeresInQuiz = answeresInQuiz + question.Id + ":" + answer.Id + "|";
                    FalseAnswer++;
                    }
                }
            AddResultQuizDto resultDto = new AddResultQuizDto();
            resultDto.Ip = ip;
            resultDto.Points = TrueAnswer;
            resultDto.MaxPoints = questionCounter;
            resultDto.QuizId = quiz.Id;
            resultDto.StartQuiz = date.AddMinutes(-20);
            resultDto.EndQuiz = date;
            resultDto.Title = quiz.Name;
            resultDto.AuthorizationResult = answeresInQuiz.ToEncodeAndHashMD5ForResultQuiz(username + quizId.ToString() + trueQuestion.ToString());
            resultDto.StudentId = studentId;
            resultDto.AnsweresInQuiz = answeresInQuiz;


            //var test = resultDto.AuthorizationResult.DecryptStringWithKey(dto.username + dto.QuizId.ToString());
            var model = _mapper.Map<Domain.Entities.Result>(resultDto);
            _context.Results.Add(model);

            var result = _context.SaveChanges();
            if (result > 0)
                {
                resultDto.Id = model.Id;
                return new ResultDto
                    {
                    IsSuccess = true,
                    Message = " با موفقیت ثبت گردید"
                    };
                }

            return new ResultDto()
                {
                IsSuccess = false,
                Message = "Warninge"
                };
            }

        public ResultDto<AddResultQuizDto> addResultQuiz(DataResultQuizDto dto)
            {
            var quiz = _context.Quizzes
                        .Where(p => p.Id == dto.QuizId)
                        .AsNoTracking()
                        .FirstOrDefault();
            var studentId = _context.Users
                               .Where(p => p.UserName == dto.username)
                               .AsNoTracking()
                               .FirstOrDefault().Id;
            if (_context.Results.Where(p => p.QuizId == dto.QuizId && p.StudentId == studentId && p.StartQuiz > quiz.StartDate && p.StartQuiz < quiz.EndDate).AsNoTracking().Any())
                {
                return new ResultDto<AddResultQuizDto>
                    {
                    Data = null,
                    IsSuccess = false,
                    Message = "Warninge"
                    };
                }
            var quizStartTemps = _context.QuizStartTemps.Where(p => p.QuizId == dto.QuizId && p.UserName == dto.username && p.RegesterAt > quiz.StartDate).FirstOrDefault();
            if (quizStartTemps == null || quizStartTemps.EndDate.AddMinutes(1) < DateTime.Now)
                {
                return new ResultDto<AddResultQuizDto>
                    {
                    Data = null,
                    IsSuccess = false,
                    Message = "Warninge"
                    };
                }
            if (dto.answer != null && dto.answer.Count() > 0)
                {
                var questiones = dto.answer.Select(p => new { key = Int64.Parse(p.Key) }).ToArray();
                var answeres = dto.answer.Select(p => new { value = Int64.Parse(p.Value) }).ToArray();

                int TrueAnswer = 0;
                int FalseAnswer = 0;
                string answeresInQuiz = "";



                for (int i = 0; i < questiones.Length; i++)
                    {

                    var answer = _context.Answers
                                                .Where(p => p.Id == answeres[i].value && p.QuestionId == questiones[i].key)
                                                .AsNoTracking()
                                                .FirstOrDefault();
                    if (answer != null)
                        {
                        if (answer.IsTrue)
                            {
                            TrueAnswer++;
                            }
                        else
                            {
                            FalseAnswer++;
                            }
                        answeresInQuiz = answeresInQuiz + questiones[i].key + ":" + answeres[i].value + "|";
                        }
                    }
                AddResultQuizDto resultDto = new AddResultQuizDto();
                resultDto.Ip = dto.Ip;
                resultDto.Points = TrueAnswer;
                resultDto.MaxPoints = quiz.MaxQuestion;
                resultDto.QuizId = quiz.Id;
                resultDto.StartQuiz = quizStartTemps.StartDate;
                resultDto.EndQuiz = DateTime.Now;
                resultDto.Title = quiz.Name;
                resultDto.AuthorizationResult = answeresInQuiz.ToEncodeAndHashMD5ForResultQuiz(dto.username + dto.QuizId.ToString() + TrueAnswer.ToString());
                resultDto.StudentId = studentId;
                resultDto.AnsweresInQuiz = answeresInQuiz;


                //var test = resultDto.AuthorizationResult.DecryptStringWithKey(dto.username + dto.QuizId.ToString());
                var model = _mapper.Map<Domain.Entities.Result>(resultDto);

                if (model.Id > 0)
                    {
                    _context.Results.Update(model);
                    }
                else
                    {
                    _context.Results.Add(model);
                    }

                var result = _context.SaveChanges();
                if (result > 0)
                    {
                    resultDto.Id = model.Id;
                    return new ResultDto<AddResultQuizDto>
                        {
                        Data = resultDto,
                        IsSuccess = true,
                        Message = "با تشکر نظر سنجی با موفقیت ثبت گردید"
                        };
                    }
                }


            //////////////////////////////////////////////////////


            return new ResultDto<AddResultQuizDto>
                {
                Data = null,
                IsSuccess = false,
                Message = "Warninge"
                };
            }
        public ResultDto deletedQuizUserAdmin(string username, long quizId)
            {
            var quiz = _context.Quizzes
                      .Where(p => p.Id == quizId)
                      .AsNoTracking()
                      .FirstOrDefault();
            var studentId = _context.Users
                           .Where(p => p.UserName == username)
                           .AsNoTracking()
                           .FirstOrDefault().Id;

            if (quiz != null && studentId != null)
                {
                if (_context.Results.Where(p => p.QuizId == quizId && p.StudentId == studentId && p.StartQuiz > quiz.StartDate && p.StartQuiz < quiz.EndDate).AsNoTracking().Any())
                    {
                    var result = _context.Results.AsNoTracking().Where(p => p.QuizId == quizId && p.StudentId == studentId && p.StartQuiz > quiz.StartDate && p.StartQuiz < quiz.EndDate).FirstOrDefault();
                    if (result != null)
                        {
                        _context.Results.Remove(result);
                        _context.SaveChanges();
                        }
                    }

                }
            var quizStartTemps = _context.QuizStartTemps.Where(p => p.QuizId == quizId && p.UserName == username && p.StartDate > quiz.StartDate && p.StartDate < quiz.EndDate).FirstOrDefault();
            if (quizStartTemps != null)
                {
                _context.QuizStartTemps.Remove(quizStartTemps);
                _context.SaveChanges();
                }
            return new ResultDto
                {
                IsSuccess = true,
                Message = "حذف با موفقیت انجام شد."
                };
            }

        public ResultDto<List<GetQuizTempDto>> GetQuizeUserStartLog(string username, long quizId)
            {
            var massageee = "";
            var quiz = _context.Quizzes
                     .Where(p => p.Id == quizId)
                     .AsNoTracking()
                     .FirstOrDefault();
            var student = _context.Users
                         .Where(p => p.UserName == username)
                         .AsNoTracking()
                         .FirstOrDefault();
            var name = student.FirstName + " " + student.LastName+" ";
            massageee += name;
            var quizStartTemps = _context.QuizStartTemps.Where(p => p.QuizId == quizId && p.UserName == username).ToList();

            if (quizStartTemps != null && quizStartTemps.Count()>0)
                {
                var model = _mapper.Map<List<GetQuizTempDto>>(quizStartTemps);
              var  Index = 0;
                foreach (var item in quizStartTemps)
                    {
                    var re = _context.Results.Where(p => p.QuizId == quizId && p.StudentId == student.Id && p.StartQuiz >= item.StartDate && p.StartQuiz <= item.EndDate).AsNoTracking().FirstOrDefault();
                    if (re!=null)
                        {
                        model.ElementAt(Index).point = re.Points;
                        model.ElementAt(Index).massage = "موفق";
                        }
                    else
                        {
                        model.ElementAt(Index).massage ="کاربر آزمون را شروع ولی به هر دلیل به پایان نرسانده است";
                        }
                    model.ElementAt(Index).FullName = name;
               Index++;
                    }
            
                return new ResultDto<List<GetQuizTempDto>>
                    {
                    Data = model,
                    IsSuccess = true,
                    Message = massageee += "کاربر وارد آزمون شده ولی آزمون را تمام ننموده و فقط سوالات را نگاه کرده است"
                    };

                }


            return new ResultDto<List<GetQuizTempDto>>
                {
                Data =null,
                IsSuccess = true,
                Message = massageee += "هیچ سابقه ای از کاربر یافت نگرید کاربر در ازمون شرکت ننموده است!!!"
                };
            }
        private IList<int> getArreyIndex(int length, int count)
            {
            IList<int> result = new List<int>();
            Random rndQusetionIndex = new Random();
            if (length < count)
                {
                count = length;
                }
            if (length == count && count == 1)
                {
                result.Add(0);
                }
            else
                {
                do
                    {
                    var a = rndQusetionIndex.Next(0, (length));
                    if (!result.Where(p => p == a).Any())
                        {
                        result.Add(a);
                        }

                    }
                while (result.Count() < count);
                return result;
                }



            return result;
            }
        private IList<long> getArreyIndexAnswer(int length, int count, List<long> Ides)
            {

            Random rndQusetionIndex = new Random();

            IList<long> result = new List<long>();
            int TrueCounter = 0;
            int FalseCounter = 0;
            do
                {
                var a = rndQusetionIndex.Next(0, (length));


                if (!result.Where(p => p == Ides[a]).Any())
                    {
                    var answers = _context.Answers
                         .Where(p => p.Id == Ides[a] && p.Status > 0)
                         .AsNoTracking()
                         .FirstOrDefault();
                    if (answers.IsTrue)
                        {
                        if (TrueCounter < 1)
                            {

                            result.Add(Ides[a]);
                            TrueCounter++;



                            }

                        }
                    else
                        {
                        if (FalseCounter < 3)
                            {

                            result.Add(Ides[a]);
                            FalseCounter++;


                            }

                        }



                    }

                }
            while (result.Count() < count);

            return result;
            }
        private List<AttemtedQuizAnswerViewModel> GetAnswerForQuestionId(long questionId)
            {
            List<AttemtedQuizAnswerViewModel> result = new List<AttemtedQuizAnswerViewModel>();
            var answers = _context.Answers
             .Where(p => p.QuestionId == questionId && p.Status == 1)
             .AsNoTracking()
             .ToList();

            var Ides = answers.Select(p => p.Id).ToList();
            var lstIndexAnswers = getArreyIndexAnswer(answers.Count(), 4, Ides);
            foreach (var item in lstIndexAnswers)
                {

                var answ = new AttemtedQuizAnswerViewModel
                    {
                    Id = answers.Find(p => p.Id == item).Id,
                    Text = answers.Find(p => p.Id == item).Text,
                    IsTrue = answers.Find(p => p.Id == item).IsTrue,
                    QuestionId = questionId
                    };
                result.Add(answ);
                }
            return result;
            }


        }
    }
