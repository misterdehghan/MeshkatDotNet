using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Controllers
{
    [Area("PublicRelations")]
    [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
    [Authorize]
    public class OperatorController : Controller
    {
        private readonly IGetOperatorServices _getOperatorServices;

        public OperatorController(IGetOperatorServices getOperatorServices)
        {
            _getOperatorServices = getOperatorServices;
        }
        public IActionResult Index(int page = 1, int pageSize = 10, string searchKey = "")
        {
            ViewBag.CurrentFilter = searchKey;

            // اطلاعات مربوط به صفحه‌بندی را به ویو منتقل می‌کنیم
            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;

            return View(_getOperatorServices.Execute(new RequestOperatorDto
            {
                page = page,
                pageSize = pageSize,
                searchKey = searchKey

            }).Data);
        }
    }
}
