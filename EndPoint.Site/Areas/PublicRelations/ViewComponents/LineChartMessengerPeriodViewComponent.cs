using Azmoon.Application.Service.PublicRelations.DashboardServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EndPoint.ViewComponents
{
    public class LineChartMessengerPeriodViewComponent : ViewComponent
    {
        private readonly IReviewingMembersBasedOnTheStatisticalPeriod _reviewingService;
        public LineChartMessengerPeriodViewComponent(IReviewingMembersBasedOnTheStatisticalPeriod reviewingService)
        {
            _reviewingService = reviewingService;
        }

        public IViewComponentResult Invoke()
        {
            var result = _reviewingService.Execute().Data;

            var viewName = $"~/Areas/PublicRelations/Views/Shared/Components/LineChartMessengerPeriod/Default.cshtml";
            return View(viewName, result); // داده‌های آماری را به ویو ارسال می‌کنیم
            }
    }
}
