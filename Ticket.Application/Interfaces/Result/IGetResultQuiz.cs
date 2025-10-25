using Azmoon.Application.Service.Result.Dto;
using Azmoon.Application.Service.Result.Query;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Interfaces.Result
{
   public interface IGetResultQuiz
    {
        ResultDto<GetResutQuizWithPager> getResultWithPager(int PageSize, int PageNo, string searchKey, int status, long guizId);
        ResultDto<GetResutQuizMyWithPager> getResultByUserId(int PageSize, int PageNo,  int status, string UserId);
        ResultDto<AddResultQuizDto> getResultByQuizId(long id);
        ResultDto<StimotReportQuizDto> getStimolReportQuizByQuizId(long id, string username);
        ResultDto<StimotReportQuizDto> getResultLottery(long id, int count, int min, int max, string username);
        List<QuizReportDro> getQuizRezultXLSX(string searchKey, int status, long guizId);
        }
}
