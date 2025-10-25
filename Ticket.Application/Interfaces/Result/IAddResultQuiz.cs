using Azmoon.Application.Service.QuizTemp.Dto;
using Azmoon.Application.Service.Result.Dto;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Interfaces.Result
{
  public  interface IAddResultQuiz
    {
        ResultDto<AddResultQuizDto>addResultQuiz(DataResultQuizDto dto);
        ResultDto addResultQuizAdmin(string username, long quizId, int questionCounter, int trueQuestion, int falseQuestion, DateTime date, string ip);
        ResultDto deletedQuizUserAdmin(string username, long quizId);     
        ResultDto<List<GetQuizTempDto>> GetQuizeUserStartLog(string username, long quizId);
        }
}
