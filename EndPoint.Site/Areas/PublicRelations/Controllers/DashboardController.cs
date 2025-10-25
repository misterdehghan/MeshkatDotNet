using Azmoon.Application.Service.PublicRelations.DashboardServices;
using Azmoon.Application.Service.PublicRelations.Period.Communication;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    public class DashboardController : Controller
    {
        private readonly IFindActiveCommunicationPeriodService _findActiveCommunicationPeriodService;
        private readonly IGetStatusVirtualSpacePeriodForDashboardService _getStatusVirtualSpacePeriodForDashboardService;



        public DashboardController(IFindActiveCommunicationPeriodService findActiveCommunicationPeriodService,
            IGetStatusVirtualSpacePeriodForDashboardService getStatusVirtualSpacePeriodForDashboardService)
        {
            _findActiveCommunicationPeriodService = findActiveCommunicationPeriodService;
            _getStatusVirtualSpacePeriodForDashboardService = getStatusVirtualSpacePeriodForDashboardService;
        }
        public IActionResult Index()
        {

            bool isActivePerformances = _findActiveCommunicationPeriodService.Execute();
            ViewBag.isActivePerformances = isActivePerformances; // ارسال نتیجه به ویو

            bool isActiveVirtualSpacePeriod = _getStatusVirtualSpacePeriodForDashboardService.Execute().IsSuccess;
            ViewBag.isActiveVirtualSpace = isActiveVirtualSpacePeriod;

            ViewBag.ActiveVirtualSpace = _getStatusVirtualSpacePeriodForDashboardService.Execute().Message;
            return View();
        }

        public IActionResult AbundanceOfChannels()
        {
            return View();
        }
    }
}
