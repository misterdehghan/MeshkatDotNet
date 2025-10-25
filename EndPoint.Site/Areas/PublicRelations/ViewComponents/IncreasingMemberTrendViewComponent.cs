using Azmoon.Application.Service.PublicRelations.DashboardServices;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.ViewComponents
{
    //روند افزایش اعضا
    public class IncreasingMemberTrendViewComponent : ViewComponent
    {
        private readonly IMemberFlow _memberFlow; 
        public IncreasingMemberTrendViewComponent(IMemberFlow memberFlow) 
        {
            _memberFlow = memberFlow; 
        }
        public IViewComponentResult Invoke() 
        { 
            var result = _memberFlow.IncreasingTrendOfMembers();
            var viewName = $"~/Areas/PublicRelations/Views/Shared/Components/IncreasingMemberTrend/Default.cshtml";
            return View(viewName, result.Data); // داده‌های آماری را به ویو ارسال می‌کنیم
            }
    }
}
