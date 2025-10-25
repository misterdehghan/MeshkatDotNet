using Azmoon.Application.Service.PublicRelations.ChannelServices;
using Azmoon.Application.Service.PublicRelations.MembersServices;
using Azmoon.Application.Service.PublicRelations.MessengerServices;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Application.Service.PublicRelations.Period.VirtualSpace;
using Azmoon.Domain.Entities;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Channel;
using EndPoint.Site.Areas.PublicRelations.Models.Dto.Membrs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EndPoint.Site.Areas.PublicRelations.Controllers
{
    [Area("PublicRelations")]
    public class ChannelController : Controller
    {
        private readonly IGetMessengerForDropDownService _getMessengerForDropDownService;
        private readonly IAddMessengerService _addMessengerService;
        private readonly IGetMessengerService _getMessengerService;
        private readonly IEditMessengerService _editMessengerService;
        private readonly IRemoveMessengerService _removeMessengerService;
        private readonly IAddChannelServices _addChannelServices;
        private readonly UserManager<User> _userManager;
        private readonly IGetOperatorForDropDownService _getOperatorForDropDownService;
        private readonly IGetMassengerNameById _getMassengerNameById;
        private readonly IGetOperatorNameById _getOperatorNameById;
        private readonly IGetListChannelServices _getChannelServices;
        private readonly IGetActiveVirtualSpacePeriodService _getActiveStatisticalPeriod;
        private readonly IAddNewMembrsService _addMembrsService;
        private readonly IGetListMembersService _getListMembersService;
        private readonly IFindChannelService _findChannelService;
        private readonly IEditChannelService _editChannelService;
        private readonly IDeleteChannelService _deleteChannelService;
        private readonly IGetNameByNormalizedNameService _getNameByNormalizedNameService;

        public ChannelController(IGetMessengerForDropDownService getMessengerForDropDownService,
            IAddMessengerService addMessengerService,
            IGetMessengerService getMessengerService,
            IEditMessengerService editMessengerService,
            IRemoveMessengerService removeMessengerService,
            IAddChannelServices addChannelServices,
            UserManager<User> userManager,
            IGetOperatorForDropDownService getOperatorForDropDownService,
            IGetMassengerNameById getMassengerNameById,
            IGetOperatorNameById getOperatorNameById,
            IGetListChannelServices getChannelServices,
            IGetActiveVirtualSpacePeriodService getActiveStatisticalPeriod,
            IAddNewMembrsService addMembrsService,
            IGetListMembersService getListMembersService,
            IFindChannelService findChannelService,
            IEditChannelService editChannelService,
            IDeleteChannelService deleteChannelService,
            IGetNameByNormalizedNameService getNameByNormalizedNameService)
        {
            _getMessengerForDropDownService = getMessengerForDropDownService;
            _addMessengerService = addMessengerService;
            _getMessengerService = getMessengerService;
            _editMessengerService = editMessengerService;
            _removeMessengerService = removeMessengerService;
            _addChannelServices = addChannelServices;
            _userManager = userManager;
            _getOperatorForDropDownService = getOperatorForDropDownService;
            _getMassengerNameById = getMassengerNameById;
            _getOperatorNameById = getOperatorNameById;
            _getChannelServices = getChannelServices;
            _getActiveStatisticalPeriod = getActiveStatisticalPeriod;
            _addMembrsService = addMembrsService;
            _getListMembersService = getListMembersService;
            _findChannelService = findChannelService;
            _editChannelService = editChannelService;
            _deleteChannelService = deleteChannelService;
            _getNameByNormalizedNameService = getNameByNormalizedNameService;
        }
        public IActionResult Index(int page = 1, int pageSize = 10, string searchKey = "")
        {
            ViewBag.CurrentFilter = searchKey;

            // اطلاعات مربوط به صفحه‌بندی را به ویو منتقل می‌کنیم
            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            bool isOperatorMatch = user.Operator == "Senior"; // بررسی شرط
            ViewBag.IsOperatorMatch = isOperatorMatch;  // ارسال نتیجه به ویو


            return View(_getChannelServices.Execute(new RequestGetChannelDto
            {
                Operator = user.Operator,
                page = page,
                pageSize = pageSize,
                searchKey = searchKey,
            }).Data);
        }

        [HttpPost]
        public IActionResult AddMessengers(string PersianName, string LatinName)
        {
            var result = _addMessengerService.Execute(PersianName, LatinName);
            return Json(result);
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult ReportOfMessengers(int page = 1, int pageSize = 10)
        {
            return View(_getMessengerService.Execute(new RequestMessengerDto
            {
                page = page,
                pageSize = pageSize,
            }).Data);
        }

        [HttpPost]
        public IActionResult EditMessenger(int Id, string PersianName, string LatinName)
        {

            return Json(_editMessengerService.Execute(new RequestEditMessengerDto
            {
                Id = Id,
                PersianName = PersianName,
                LatinName = LatinName
            }));
        }

        [HttpPost]
        public IActionResult RemoveMessenger(int MessengerId)
        {

            return Json(_removeMessengerService.Execute(MessengerId));
        }

        [HttpGet]
        public IActionResult AddChannel()
        {
            ViewBag.Messenger = new SelectList(_getMessengerForDropDownService.Execute().Data, "Id", "Name");
            ViewBag.Operator = new SelectList(_getOperatorForDropDownService.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddChannel(AddChannlDto addChannlDto)
        {

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                if (user.Operator == "Senior" && ModelState.IsValid)
                {
                    var messengerName = _getMassengerNameById.Execute(addChannlDto.MessengersName);
                    var operatorName = _getOperatorNameById.Execute(addChannlDto.Operator);
                    _addChannelServices.Execute(new ForAddChannlDto
                    {
                        MessengersName = messengerName,
                        ChannelName = addChannlDto.ChannelName,
                        PhoneNumber = addChannlDto.PhoneNumber,
                        Address = addChannlDto.Address,
                        ActivationDate = addChannlDto.ActivationDate,
                        Operator = _getNameByNormalizedNameService.Execute(operatorName).Data,
                    });
                    return Json(new { isSuccess = true, message = "عملیات با موفقیت انجام شد." });

                }
                if (user.Operator != "Senior" && ModelState.IsValid)
                {
                    var messengerName = _getMassengerNameById.Execute(addChannlDto.MessengersName);
                    _addChannelServices.Execute(new ForAddChannlDto
                    {
                        MessengersName = messengerName,
                        ChannelName = addChannlDto.ChannelName,
                        PhoneNumber = addChannlDto.PhoneNumber,
                        Address = addChannlDto.Address,
                        ActivationDate = addChannlDto.ActivationDate,
                        Operator = _getNameByNormalizedNameService.Execute(user.Operator).Data,
                    });
                    return Json(new { isSuccess = true, message = "عملیات با موفقیت انجام شد." });
                }
            }

            return Json(new { isSuccess = false, message = "لطفاً تمام فیلدها را به درستی پر کنید." });

        }

        public IActionResult AddMembers(ForAddMembrDto dto)
        {
            var activeStatistical = _getActiveStatisticalPeriod.Execute().Data.Id;
            var result = _addMembrsService.Execute(new RequestMembrsDto
            {
                Member = dto.Member,
                ChannelId = dto.ChannelId,
                VirtualSpacePeriodId = activeStatistical
            });
            return Json(result);
        }

        [HttpGet]
        public IActionResult PartialViewListMemberModal(int channelId)
        {
            var result = _getListMembersService.Execute(new RequestListMembersDto
            {
                ChannelId = channelId,

            }).Data;
            return PartialView(result);
        }

        [HttpGet]
        public IActionResult PartialViewDetailChannelModal(int channelId)
        {
            var result = _findChannelService.Execute(channelId);

            if (!result.IsSuccess)
            {
                // در صورت عدم موفقیت می‌توانیم یک پیام خطا یا ویو دیگری برگردانیم
                return PartialView("Error", result.Message);
            }

            GetChannelDetailDto getChannel = new GetChannelDetailDto
            {
                Id = result.Data.Id,
                InsertTime = result.Data.InsertTime,
                UpdateTime = result.Data.UpdateTime,
                IsRemoved = result.Data.IsRemoved,
                RemoveTime = result.Data.RemoveTime,
                MessengersName = result.Data.MessengersName,
                ChannelName = result.Data.ChannelName,
                PhoneNumber = result.Data.PhoneNumber,
                Address = result.Data.Address,
                ActivationDate = result.Data.ActivationDate,
                Operator = result.Data.Operator
            };
            var channel = result.Data;

            return PartialView(getChannel);
        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult PartialViewEditChannelModal(int channelId)
        {
            ViewBag.Messenger = new SelectList(_getMessengerForDropDownService.Execute().Data, "Id", "Name");
            var result = _findChannelService.Execute(channelId).Data;

            GetChannelEditDto getChannelEdit = new GetChannelEditDto
            {
                Id = result.Id,
                InsertTime = result.InsertTime,
                UpdateTime = result.UpdateTime,
                IsRemoved = result.IsRemoved,
                RemoveTime = result.RemoveTime,
                MessengersName = result.MessengersName,
                ChannelName = result.ChannelName,
                PhoneNumber = result.PhoneNumber,
                Address = result.Address,
                ActivationDate = result.ActivationDate,
                Operator = result.Operator
            };
            return PartialView(getChannelEdit);
        }

        [HttpPost]
        public IActionResult PartialViewEditChannelModal(PostChannelEditDto postChannelEdit)
        {

            var messengerName = _getMassengerNameById.Execute(postChannelEdit.MessengersName);
            var channelEdit = new ForChannelEdit
            {
                Id = postChannelEdit.Id,
                MessengersName = messengerName, // نام پیام‌رسان
                ChannelName = postChannelEdit.ChannelName, // نام کانال
                PhoneNumber = postChannelEdit.PhoneNumber, // شماره تلفن
                Address = postChannelEdit.Address // آدرس

            };


            var result = _editChannelService.Execute(channelEdit);
            return Json(new { isSuccess = true, message = "ویرایش با موفقیت انجام شد." });


        }

        [Authorize(Roles = "SuperUserAccountPR,AdminPR")]
        public IActionResult PartialViewDeleteChannelModal(int channelId)
        {
            var result = _findChannelService.Execute(channelId);

            if (!result.IsSuccess)
            {
                // در صورت عدم موفقیت می‌توانیم یک پیام خطا یا ویو دیگری برگردانیم
                return PartialView("Error", result.Message);
            }

            GetChannelDetailDto getChannel = new GetChannelDetailDto
            {
                Id = result.Data.Id,
                InsertTime = result.Data.InsertTime,
                UpdateTime = result.Data.UpdateTime,
                IsRemoved = result.Data.IsRemoved,
                RemoveTime = result.Data.RemoveTime,
                MessengersName = result.Data.MessengersName,
                ChannelName = result.Data.ChannelName,
                PhoneNumber = result.Data.PhoneNumber,
                Address = result.Data.Address,
                ActivationDate = result.Data.ActivationDate,
                Operator = result.Data.Operator
            };
            var channel = result.Data;

            return PartialView(getChannel);
        }

        [HttpPost]
        public IActionResult PartialViewDeleteChannelModal(GetChannelDeleteDto channelDeleteDto)
        {
            var result = _deleteChannelService.Execute(channelDeleteDto.Id);

            if (result.IsSuccess)
            {
                return Json(new { isSuccess = true, message = "عملیات حذف با موفقیت انجام شد" });
            }
            return Json(new { isSuccess = false, message = "خطایی رخ داده است" });

        }

    }
}
