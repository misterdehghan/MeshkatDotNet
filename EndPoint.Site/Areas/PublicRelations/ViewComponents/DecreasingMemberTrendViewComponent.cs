using Azmoon.Application.Service.PublicRelations.DashboardServices;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.ViewComponents
{
    public class DecreasingMemberTrendViewComponent : ViewComponent
    {
        private readonly IMemberFlow _memberFlow;

        public DecreasingMemberTrendViewComponent(IMemberFlow memberFlow)
        {
            _memberFlow = memberFlow;
        }

        public IViewComponentResult Invoke()
        {
            var result = _memberFlow.DecreasingTrendOfMembers();
            var viewName = $"~/Areas/PublicRelations/Views/Shared/Components/DecreasingMemberTrend/Default.cshtml";
            var viewError = $"~/Areas/PublicRelations/Views/Shared/Error.cshtml";
            //if (!result.IsSuccess || result.Data == null)
            //{
            //    return View(viewError, result.Message); // نمایش پیغام خطا در صورت عدم موفقیت
            //}

            return View(viewName, result.Data); // داده‌های آماری را به ویو ارسال می‌کنیم
            }
    }
}
