using Application.Services.Location.Province;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Location.Province
{
    public interface IGetProvinceService
    {
        ResultDto<List<ProvinceDto>> Execute();

    }
}
