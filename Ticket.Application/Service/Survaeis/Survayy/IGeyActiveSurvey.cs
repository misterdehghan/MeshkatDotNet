using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Survayy
{
   public interface IGeyActiveSurvey
    {
        List<GetSurvayViewModel> getSurvays(int survaieType);
    }
    public class GeyActiveSurvey : IGeyActiveSurvey
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public GeyActiveSurvey(IDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<GetSurvayViewModel> getSurvays(int survaieType)
        {
 
            var db = _context.Surveys.Where(p => p.Status == 1  && p.EndDate >= DateTime.Now.AddDays(-10) && p.StartDate<=DateTime.Now).AsNoTracking()
                   .Include(p => p.Group).AsQueryable();
            var result = _mapper.ProjectTo<GetSurvayViewModel>(db).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                var start = (DateTime)result.ElementAt(i).StartDate;
                var startSuspand = start.AddHours(5.0);
                var end = (DateTime)result.ElementAt(i).EndDate;
                if (start <= DateTime.Now && end > DateTime.Now)
                {
                    //در حال برگزاری
                    result.ElementAt(i).Status = 1;
                }
                if (startSuspand >= DateTime.Now && start > DateTime.Now)
                {
                    //در انتظار شروع
                    result.ElementAt(i).Status = 2;
                }
                if (end < DateTime.Now)
                {
                    //خاتمه یافته
                    result.ElementAt(i).Status = 3;
                }
            }
            return result;
        }
    }
}
