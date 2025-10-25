
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Location.City
{
    public interface IGetCityService
    {
        ResultDto<List<CityDto>> Execute(int ProvinceId);
    }

    public class CityDto
    {
        public int Id { get; set; }
        public string NameCity { get; set; }

    }

    public class GetCityService : IGetCityService
    {
        private readonly IDataBaseContext _context;

        public GetCityService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<CityDto>> Execute(int ProvinceId)
        {
            var City= _context.Cities.Where(p=> p.ProvinceId== ProvinceId).ToList().Select(
                p=> new CityDto
                {
                    Id=p.Id,
                    NameCity = p.Name,
                }).ToList();
            return new ResultDto<List<CityDto>>()
            {
                Data = City,
                IsSuccess = true,
                Message = "",
            };
        }
    }
}
