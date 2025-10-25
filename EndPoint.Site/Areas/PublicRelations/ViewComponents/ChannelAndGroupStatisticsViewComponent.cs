using Azmoon.Application.Service.PublicRelations.DashboardServices;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.ViewComponents
{
    public class ChannelAndGroupStatisticsViewComponent : ViewComponent
    {
        private readonly IChannelAndGroupStatisticsServices _channelAndGroupStatisticsServices;

        public ChannelAndGroupStatisticsViewComponent(IChannelAndGroupStatisticsServices channelAndGroupStatisticsServices)
        {
            _channelAndGroupStatisticsServices = channelAndGroupStatisticsServices;
        }

        public IViewComponentResult Invoke()
        {
            var result = _channelAndGroupStatisticsServices.Execute();
            var viewName = $"~/Areas/PublicRelations/Views/Shared/Components/ChannelAndGroupStatistics/Default.cshtml";
            return View(viewName, result.Data); // داده‌های آماری را به ویو ارسال می‌کنیم
        }
    }
}
