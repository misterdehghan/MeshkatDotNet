using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.NewsPerformancesServices
{
    public interface IDeleteNewsPerformancesService
    {
        ResultDto Execute(int id);
    }

    public class DeleteNewsPerformancesService : IDeleteNewsPerformancesService
    {
        private readonly IDataBaseContext _context;

        public DeleteNewsPerformancesService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int id)
        {
            var newsPerformances = _context.NewsPerformances.FirstOrDefault(p => p.Id == id);
            if (newsPerformances == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رکورد یافت نشد"
                };
            }
            newsPerformances.IsRemoved = true;
            newsPerformances.RemoveTime = DateTime.Now;
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "رکورد با موفقیت حذف شد"
            };
        }
    }
}
