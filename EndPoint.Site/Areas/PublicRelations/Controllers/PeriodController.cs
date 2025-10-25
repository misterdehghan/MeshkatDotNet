using Azmoon.Application.Service.PublicRelations.Period.Communication;
using Azmoon.Application.Service.PublicRelations.Period.VirtualSpace;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Period;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;

namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    [Authorize]
    public class PeriodController : Controller
    {
        private readonly IGetListVirtualSpacePeriodService _getStatisticalPeriod;
        private readonly IAddVirtualSpacePeriodService _addNewStatisticalPeriod;
        private readonly IGetDetailVirtualSpacePeriodService _getDetailStatisticalPeriodService;
        private readonly IEditVirtualSpacePeriodService _editStatisticsPeriodServices;
        private readonly IAddCommunicationPeriodService _addCommunicationPeriodService;
        private readonly IGetListCommunicationPeriodService _getListCommunicationPeriodService;
        private readonly IDeleteCommunicationPeriodService _deleteCommunicationPeriodService;
        private readonly IDeleteVirtualSpacePeriodService _deleteVirtualSpacePeriodService;
        private readonly IDetailsCommunicationPeriodService _detailsCommunicationPeriodService;
        private readonly IEditCommunicationPeriodService _editCommunicationPeriodService;

        public PeriodController(IGetListVirtualSpacePeriodService getStatisticalPeriod,
             IAddVirtualSpacePeriodService addNewStatisticalPeriod,
             IGetDetailVirtualSpacePeriodService getDetailStatisticalPeriodService,
             IEditVirtualSpacePeriodService editStatisticsPeriodServices,
             IAddCommunicationPeriodService addCommunicationPeriodService,
             IGetListCommunicationPeriodService getListCommunicationPeriodService,
             IDeleteCommunicationPeriodService deleteCommunicationPeriodService,
             IDeleteVirtualSpacePeriodService deleteVirtualSpacePeriodService,
             IDetailsCommunicationPeriodService detailsCommunicationPeriodService,
             IEditCommunicationPeriodService editCommunicationPeriodService)
        {
            _getStatisticalPeriod = getStatisticalPeriod;
            _addNewStatisticalPeriod = addNewStatisticalPeriod;
            _getDetailStatisticalPeriodService = getDetailStatisticalPeriodService;
            _editStatisticsPeriodServices = editStatisticsPeriodServices;
            _addCommunicationPeriodService = addCommunicationPeriodService;
            _getListCommunicationPeriodService = getListCommunicationPeriodService;
            _deleteCommunicationPeriodService = deleteCommunicationPeriodService;
            _deleteVirtualSpacePeriodService = deleteVirtualSpacePeriodService;
            _detailsCommunicationPeriodService = detailsCommunicationPeriodService;
            _editCommunicationPeriodService = editCommunicationPeriodService;
        }
        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult IndexVirtualSpace(int page = 1, int pageSize = 10, string searchKey = "")
        {
            ViewBag.CurrentFilter = searchKey;

            // اطلاعات مربوط به صفحه‌بندی را به ویو منتقل می‌کنیم
            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;

            return View(_getStatisticalPeriod.Execute(new RequestPeriodDto
            {
                page = page,
                pageSize = pageSize,
                searchKey = searchKey,
            }).Data);
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult AddStatisticalPeriodVirtualSpace()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStatisticalPeriodVirtualSpace(AddStatisticalPeriodDto addStatisticalPeriodDto)
        {
            if (ModelState.IsValid)
            {

                var result = _addNewStatisticalPeriod.Execute(new ForAddStatisticalPeriodDto
                {
                    StatisticalPeriod = addStatisticalPeriodDto.StatisticalPeriod,
                    StartDate = addStatisticalPeriodDto.StartDate,
                    EndDate = addStatisticalPeriodDto.EndDate
                });

                return Json(new { result.IsSuccess, result.Message });

            }

            return Json(new { isSuccess = false, message = "عملیات با موفقیت انجام نشد." });
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult EditVirtualSpace(int PeriodId)
        {
            var result = _getDetailStatisticalPeriodService.Execute(PeriodId).Data;
            GetDetailStatisticalPeriodDto getDetail = new GetDetailStatisticalPeriodDto
            {
                Id = result.Id,
                InsertTime = result.InsertTime,
                UpdateTime = result.UpdateTime,
                StatisticalPeriod = result.StatisticalPeriod,
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                IsActive = result.IsActive
            };
            return View(getDetail);
        }

        [HttpPost]
        public IActionResult EditVirtualSpace(ForPeriodEdit periodEdit)
        {
            var result = _editStatisticsPeriodServices.Execute(new ForPeriodEdit
            {
                Id = periodEdit.Id,
                StatisticalPeriod = periodEdit.StatisticalPeriod,
                StartDate = periodEdit.StartDate,
                EndDate = periodEdit.EndDate
            });
            return Json(result);
        }

        //------
        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult PartialViewDeletePeriodVirtualSpaceModal(int PeriodId)
        {
            var result = _getDetailStatisticalPeriodService.Execute(PeriodId).Data;
            GetDeleteStatisticalPeriodDto getDetail = new GetDeleteStatisticalPeriodDto
            {
                Id = result.Id,
                InsertTime = result.InsertTime,
                UpdateTime = result.UpdateTime,
                StatisticalPeriod = result.StatisticalPeriod,
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                IsActive = result.IsActive
            };
            return PartialView(getDetail);
        }

        public IActionResult DeletePeriodVirtualSpace(int PeriodId)
        {
            var result = _deleteVirtualSpacePeriodService.Execute(PeriodId);
            return Json(new
            {
                result.IsSuccess,
                result.Message
            });
        }
        //------

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult AddStatisticalPeriodPerformance()
        {
            return View();
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        [HttpPost]
        public IActionResult AddStatisticalPeriodPerformance(AddStatisticalPeriodPerformanceDto addStatisticalPeriodPerformanceDto)
        {
            if (ModelState.IsValid)
            {

                var result = _addCommunicationPeriodService.Execute(new RequestStatisticalCourseNP
                {
                    StatisticalPeriod = addStatisticalPeriodPerformanceDto.StatisticalPeriod,
                    StartDate = addStatisticalPeriodPerformanceDto.StartDate,
                    EndDate = addStatisticalPeriodPerformanceDto.EndDate
                });

                return Json(new { result.IsSuccess, result.Message });

            }
            return Json(new { isSuccess = false, message = "عملیات با موفقیت انجام نشد." });

        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult IndexPerformance(int page = 1, int pageSize = 10, string searchKey = "")
        {
            ViewBag.CurrentFilter = searchKey;

            // اطلاعات مربوط به صفحه‌بندی را به ویو منتقل می‌کنیم
            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;

            return View(_getListCommunicationPeriodService.Execute(new RequestPeriodNPDto
            {
                page = page,
                pageSize = pageSize,
                searchKey = searchKey,
            }).Data);
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult DeletePeriodPerformanceModal(int PeriodId)
        {
            var result = _deleteCommunicationPeriodService.Execute(PeriodId);
            return Json(new
            {
                result.IsSuccess,
                result.Message
            });
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult EditPerformance(int id)
        {
            var result = _detailsCommunicationPeriodService.Execute(id);
            return View(result.Data);
        }

        [HttpPost]
        public IActionResult EditPerformance(RequestEditCommunicationPeriodDto request)
        {


            var result = _editCommunicationPeriodService.Execute(new RequestEditCommunicationPeriod
            {
                Id = request.Id,
                StatisticalPeriod = request.StatisticalPeriod,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            });

            return Json(result);
        }

    }
}
