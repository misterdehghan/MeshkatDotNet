using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IGetSubjectForDropDownService
    {
        ResultDto<List<SubjectForDropDownDto>> Execute();
    }

    public class SubjectForDropDownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GetSubjectForDropDownService : IGetSubjectForDropDownService
    {
        private readonly IDataBaseContext _context;
        public GetSubjectForDropDownService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<SubjectForDropDownDto>> Execute()
        {
            var subjectsList = _context.Subjects
        .Select(p => new SubjectForDropDownDto
        {
            Id = p.Id,
            Name = p.Title,
        }).ToList();

            if (subjectsList == null || !subjectsList.Any())
            {
                return new ResultDto<List<SubjectForDropDownDto>>
                {
                    IsSuccess = false,
                    Data = new List<SubjectForDropDownDto>(),
                    Message = "هیچ موضوعی ثبت نشده است."
                };
            }

            return new ResultDto<List<SubjectForDropDownDto>>
            {
                IsSuccess = true,
                Data = subjectsList,
                Message = ""
            };
        }
    }
}
