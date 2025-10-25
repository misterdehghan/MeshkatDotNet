using Azmoon.Application.Interfaces.Contexts;
using System.Linq;

namespace Application.Services.Location.Province
{
    public interface IGetProvinceNameById
    {
        string Execute(string id);
    }

    public class GetProvinceNameById : IGetProvinceNameById
    {
        private readonly IDataBaseContext _context;
        public GetProvinceNameById(IDataBaseContext context)
        {
           _context = context;
        }

        public string Execute(string id)
        {
            if (int.TryParse(id, out int provinceId))
            {
                var province = _context.Provinces.FirstOrDefault(p => p.Id == provinceId);
                if (province != null)
                {
                    return province.Name;
                }
            }
            return null; 
        }


    }
}
