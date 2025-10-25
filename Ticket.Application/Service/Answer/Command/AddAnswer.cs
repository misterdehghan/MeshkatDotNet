using AutoMapper;
using Azmoon.Application.Interfaces.Answer;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Answer.Dto;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Answer.Command
{
    public class AddAnswer : IAddAnswer
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public AddAnswer(IDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ResultDto<AddAnswerDto> Exequte(AddAnswerDto dto , string userName)
        {
            var user = _context.Users.AsNoTracking().Where(p => p.UserName == userName).FirstOrDefault();
           
            var answer = _mapper.Map<Domain.Entities.Answer>(dto);
            answer.UserId = user.Id;

            if (answer.Id > 0)
            {
                var dbAnswer = _context.Answers.AsNoTracking().Where(p => p.Id == dto.Id).FirstOrDefault();
                dbAnswer.IsTrue = dto.IsTrue;
                dbAnswer.Text = dto.Text;
                dbAnswer.UserId = user.Id;
                _context.Answers.Update(dbAnswer);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return new ResultDto<AddAnswerDto>
                    {
                        Data = dto,
                        IsSuccess = true,
                        Message = "موفق"
                    };
                }
            }
            else
            {
                var answerAdd = new Azmoon.Domain.Entities.Answer { 
                Id=0,
                IsTrue=dto.IsTrue,
                UserId=user.Id,
                QuestionId=dto.QuestionId,
                Text=dto.Text
                };
                _context.Answers.Add(answerAdd);
                var resultAdd = _context.SaveChanges();
                if (resultAdd > 0)
                {
                    return new ResultDto<AddAnswerDto>
                    {
                        Data = dto,
                        IsSuccess = true,
                        Message = "موفق"
                    };
                }
            }

           
            return new ResultDto<AddAnswerDto>
            {
                Data = null,
                IsSuccess = false,
                Message = "نا موفق"
            };
        }
    }
}
