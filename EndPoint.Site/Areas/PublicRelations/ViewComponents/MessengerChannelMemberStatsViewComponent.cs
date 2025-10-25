using Azmoon.Application.Service.PublicRelations.DashboardServices;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.ViewComponents
{
    public class MessengerMemberStatisticsViewComponent : ViewComponent
    {
        private readonly IGetTotalMembersFromMessengersService _getTotalMembersService;

        public MessengerMemberStatisticsViewComponent(IGetTotalMembersFromMessengersService getTotalMembersService)
        {
            _getTotalMembersService = getTotalMembersService;
        }

        public IViewComponentResult Invoke()
        {
            var result = _getTotalMembersService.Execute();

            var viewName = $"~/Areas/PublicRelations/Views/Shared/Components/MessengerMemberStatistics/Default.cshtml";
            return View(viewName, result.Data); // داده‌های آماری را به ویو ارسال می‌کنیم
            }
    }
}
