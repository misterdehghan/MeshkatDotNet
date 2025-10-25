using Azmoon.Application.Service.PublicRelations.DashboardServices;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Doshbord;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.ViewComponents
{
    public class StatisticsForDoshbord : ViewComponent
    {
        private readonly IChannelAndGroupStatisticsServices _channelAndGroupStatisticsServices;
        private readonly IGetTotalMembersFromMessengersService _getTotalMembersFromMessengersService;
        public StatisticsForDoshbord(IChannelAndGroupStatisticsServices channelAndGroupStatisticsServices,
            IGetTotalMembersFromMessengersService getTotalMembersFromMessengersService)
        {
            _channelAndGroupStatisticsServices = channelAndGroupStatisticsServices;
            _getTotalMembersFromMessengersService = getTotalMembersFromMessengersService;
        }



        public IViewComponentResult Invoke()
        {
            var result = _channelAndGroupStatisticsServices.Execute().Data;
            var resultTotal = _getTotalMembersFromMessengersService.Execute().Data;

            ChannelAndGroupStatisticsDto statisticsDto = new ChannelAndGroupStatisticsDto
            {
                whatsapp = result.whatsapp,
                telegram = result.telegram,
                Instagram = result.Instagram,
                x = result.x,
                sorosh = result.sorosh,
                iGap = result.iGap,
                eeta = result.eeta,
                bale = result.bale,
                rubika = result.rubika,
                total = result.total,

                TotalMembersWhatsapp = resultTotal.whatsapp,
                TotalMembersTelegram=resultTotal.telegram,
                TotalMembersInstagram=resultTotal.Instagram,
                TotalMembersX=resultTotal.x,
                TotalMembersSorosh=resultTotal.sorosh,
                TotalMembersIGap=resultTotal.iGap,
                TotalMembersEeta=resultTotal.eeta,
                TotalMembersBale=resultTotal.bale,
                TotalMembersRubika=resultTotal.rubika,
                TotalMembers=resultTotal.total
            };

            var viewName = $"~/Areas/PublicRelations/Views/Shared/Components/StatisticsForDoshbord/ChannelAndGroupStatistics.cshtml";
            return View(viewName, statisticsDto); // داده‌های آماری را به ویو ارسال می‌کنیم

        }
    }
}
