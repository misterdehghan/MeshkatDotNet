
using Azmoon.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.Location.City
{
    public interface IGetCityNameById
    {
        string Execute(string id);
    }

    public class GetCityNameById : IGetCityNameById
    {
        private readonly IDataBaseContext _context;

        public GetCityNameById(IDataBaseContext context)
        {
            _context = context;
        }
        public string Execute(string id)
        {
            if (int.TryParse(id, out int provinceId))
            {
                var city = _context.Cities.FirstOrDefault(p => p.Id == provinceId);
                if (city != null)
                {
                    return city.Name;
                }
            }
            return null;
        }
    }
}
