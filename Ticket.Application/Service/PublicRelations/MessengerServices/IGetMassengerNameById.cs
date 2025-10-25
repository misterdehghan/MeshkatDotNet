using Azmoon.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.MessengerServices
{
    public interface IGetMassengerNameById
    {
        string Execute(string id);
    }

    public class GetMassengerNameById : IGetMassengerNameById
    {
        private readonly IDataBaseContext _context;

        public GetMassengerNameById(IDataBaseContext context)
        {
            _context = context;
        }
        public string Execute(string id)
        {
            if (int.TryParse(id, out int massengerId))
            {
                var massenger = _context.Messengers.FirstOrDefault(p => p.Id == massengerId);
                if (massenger != null)
                {
                    return massenger.PersianName;
                }
            }
            return null;
        }
    }
}
