using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IDeleteMediaPerformancesService
    {
        ResultDto Execute(int id);
    }
    public class DeleteMediaPerformancesService : IDeleteMediaPerformancesService
    {
        private readonly IDataBaseContext _context;

        public DeleteMediaPerformancesService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int id)
        {
            var mediaPerformances = _context.MediaPerformances.FirstOrDefault(p => p.Id == id);
            if (mediaPerformances == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رکورد یافت نشد"
                };
            }
            mediaPerformances.IsRemoved = true;
            mediaPerformances.RemoveTime = DateTime.Now;
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "رکورد با موفقیت حذف شد"
            };
        }
    }

}
