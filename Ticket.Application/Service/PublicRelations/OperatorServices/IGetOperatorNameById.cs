using Azmoon.Application.Interfaces.Contexts;
using System;
using System.Linq;

namespace Azmoon.Application.Service.PublicRelations.OperatorServices
{
    public interface IGetOperatorNameById
    {
        string Execute(string id);
    }

    public class GetOperatorNameById : IGetOperatorNameById
    {
        private readonly IDataBaseContext _context;
        public GetOperatorNameById(IDataBaseContext context)
        {
            _context = context;
        }

        public string Execute(string id)
        {
            if (Guid.TryParse(id, out Guid operatorId))
            {
                var operatorObject = _context.Operators.FirstOrDefault(p => p.Id == operatorId);
                if (operatorObject != null)
                {
                    return operatorObject.NormalizedName;
                }
            }
            return null;
        }
    }
}
