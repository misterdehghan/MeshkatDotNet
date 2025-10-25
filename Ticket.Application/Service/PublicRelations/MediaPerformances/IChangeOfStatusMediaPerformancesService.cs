using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MediaPerformances
{
    public interface IChangeOfStatusMediaPerformancesService
    {
        ResultDto Execute(int id, string Operator);
    }

    public class ChangeOfStatusMediaPerformancesService : IChangeOfStatusMediaPerformancesService
    {
        private readonly IDataBaseContext _context;
        public ChangeOfStatusMediaPerformancesService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(int id, string Operator)
        {
            var mediaPerformances = _context.MediaPerformances.FirstOrDefault(p => p.Id == id);
            if (Operator != "Senior")
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما مجاز به تغییر وضعیت نمی باشید"
                };
            }
            if (mediaPerformances == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رکورد یافت نشد"
                };
            }


            // تغییر وضعیت Confirmation به حالت مخالف
            mediaPerformances.Confirmation = !mediaPerformances.Confirmation;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "وضعیت تغییر یافت"
            };
        }
    }
}
