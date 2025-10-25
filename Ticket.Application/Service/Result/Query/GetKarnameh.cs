using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Result.Dto;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Result.Query
{
   public interface IGetKarnameh
    {
        ResultDto<List<GetKarnamehViewModel>> getKarnameh(long resultId);
    }
    public class GetKarnameh : IGetKarnameh
    {
        private readonly IDataBaseContext _context;

        public GetKarnameh(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<GetKarnamehViewModel>> getKarnameh(long resultId)
        {
            var result = _context.Results.AsNoTracking()
           .Where(p => p.Id == resultId).FirstOrDefault();
            if (result!=null)
            {
                List<GetKarnamehViewModel> data = new List<GetKarnamehViewModel>();
                var userAnsweres = result.AnsweresInQuiz.Split('|');
                foreach (var item in userAnsweres)
                {
                    var ab = item.Split(':');
                    if (ab.Count()>0 && ab[0]!="")
                    {
        var questionText = _context.Qestions.AsNoTracking()
                              .Where(p => p.Id == Int64.Parse(ab[0])).FirstOrDefault().Text;

                    var answer = _context.Answers.AsNoTracking()
                              .Where(p => p.Id == Int64.Parse(ab[1])).FirstOrDefault();

                    data.Add(new GetKarnamehViewModel {
                    questionText= questionText,
                    userAnswer=answer.Text,
                    IsTrue=answer.IsTrue
                    });
                    }
                
            

                }
                return new ResultDto<List<GetKarnamehViewModel>>
                {

                    Data = data,
                    IsSuccess = true,
                    Message = "عملیات موفق"
                };
            }
            return new ResultDto<List<GetKarnamehViewModel>>
            {
                Data = null,
                IsSuccess = false,
                Message = "مشکلی در ارسال داده ها"
            };
        }
    }
}
