using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.PublicRelations.DashboardServices
{
    public interface IChannelAndGroupStatisticsServices
    {
        ResultDto<ResultChannelAndGroup> Execute();
    }

    public class ResultChannelAndGroup
    {
        public int whatsapp { get; set; }
        public int telegram { get; set; }
        public int Instagram { get; set; }
        public int x { get; set; }
        public int sorosh { get; set; }
        public int iGap { get; set; }
        public int eeta { get; set; }
        public int bale { get; set; }
        public int rubika { get; set; }
        public int total { get; set; }


    }

    public class ChannelAndGroupStatisticsServices : IChannelAndGroupStatisticsServices
    {
        private readonly IDataBaseContext _context;
        public ChannelAndGroupStatisticsServices(IDataBaseContext context)
        {
            _context = context;
        }

        ResultDto<ResultChannelAndGroup> IChannelAndGroupStatisticsServices.Execute()
        {
            var whatsapp = _context.Channels.Count(w => w.MessengersName == "واتس آپ");
            var telegram = _context.Channels.Count(w => w.MessengersName == "تلگرام");
            var Instagram = _context.Channels.Count(w => w.MessengersName == "اینستاگرام");
            var x = _context.Channels.Count(w => w.MessengersName == "ایکس");
            var sorosh = _context.Channels.Count(w => w.MessengersName == "سروش پلاس");
            var iGap = _context.Channels.Count(w => w.MessengersName == "آی گپ");
            var eeta = _context.Channels.Count(w => w.MessengersName == "ایتا");
            var bale = _context.Channels.Count(w => w.MessengersName == "بله");
            var rubika = _context.Channels.Count(w => w.MessengersName == "روبیکا");
            var total = _context.Channels.Count();


            var resultChannelAndGroup = new ResultChannelAndGroup
            {
                whatsapp = whatsapp,
                telegram = telegram,
                Instagram = Instagram,
                x = x,
                sorosh = sorosh,
                iGap = iGap,
                eeta = eeta,
                bale = bale,
                rubika = rubika,
                total = total
            };



            return new ResultDto<ResultChannelAndGroup>
            {
                IsSuccess = true,
                Data = resultChannelAndGroup,
                Message = "درخواست با موفقیت انجام شد"
            };


        }
    }
}
