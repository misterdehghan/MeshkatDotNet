

using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities.PublicRelations.Main;

namespace Azmoon.Application.Service.PublicRelations.MessengerServices
{
    public interface IAddMessengerService
    {
        ResultDto Execute(string PersianName, string LatinName);
    }

    public class AddMessengerService : IAddMessengerService
    {
        private readonly IDataBaseContext _context;

        public AddMessengerService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(string PersianName, string LatinName)
        {
            Messenger messenger = new Messenger
            {
                IsRemoved = false,
                PersianName = PersianName,
                LatinName = LatinName
            };

            _context.Messengers.Add(messenger);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "با موفقیت ذخیره شد"
            };
        }
    }

}
