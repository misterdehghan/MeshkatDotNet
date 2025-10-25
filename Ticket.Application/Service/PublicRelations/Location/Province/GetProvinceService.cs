using Application.Services.Location.Province;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Location.Province
{
    public class GetProvinceService : IGetProvinceService
    {

        private readonly IDataBaseContext _context;
        public GetProvinceService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<ProvinceDto>> Execute()
        {
            var Province = _context.Provinces.ToList().Select(p => new ProvinceDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return new ResultDto<List<ProvinceDto>>()
            {
                Data = Province,
                IsSuccess = true,
                Message = "",
            };
        }
    }
}
