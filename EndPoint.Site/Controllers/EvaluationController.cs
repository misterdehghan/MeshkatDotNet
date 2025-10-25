using Azmoon.Application.Interfaces.Assessment;
using Azmoon.Application.Service.Assessment;
using Azmoon.Application.Service.Survaeis.Results;
using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Common.ResultDto;
using DotNet.RateLimiter.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using Microsoft.AspNetCore.Identity;
using Azmoon.Domain.Entities;
using Azmoon.Application.Service.Assessment.Dto;

namespace EndPoint.Site.Controllers
    {
    public class EvaluationController : Controller
        {
        private readonly SignInManager<User> _signInManager;
        private readonly IStarClientAssessment _starClient;
        private readonly IGetAssessment _getAssessment;
        private readonly IAddUserAnswerInAssessment _addUserAnswerInAssessment;
        private string userId = null;
        public EvaluationController(IStarClientAssessment starClient,
            IGetAssessment getAssessment,
            SignInManager<User> signInManager,
            IAddUserAnswerInAssessment addUserAnswerInAssessment)
            {
            _starClient = starClient;
            _getAssessment = getAssessment;
            _signInManager = signInManager;
            _addUserAnswerInAssessment = addUserAnswerInAssessment;
            }
        public IActionResult Index(long wpId)
            {
            var model = _getAssessment.GetAssessmentWpId(wpId);
            return View(model.Data);
            }
        [HttpPost]
        public IActionResult Start(int assessmentId)
            {
            var model = _starClient.GetStarClientAssessment(assessmentId);
            if (model == null)
                {
                return RedirectToAction("Index", "Home");
                }
            return View(model);
            }

        [HttpPost]
        [RateLimit(PeriodInSec = 30, Limit = 4)]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(IFormCollection formCollection, long WorkPlaceId)
            {
            if (!ModelState.IsValid)
                {

                return RedirectToAction("AccessDenied", "Account", new { area = "", message = "شما بصورت غیر مجاز قصد شرکت  در نظرسنجی را داشته اید!!!!" });

                }
            var ip = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");
            if (String.IsNullOrEmpty(ip))
                {
                IPAddress ipAddress;
                var headers = Request.Headers.ToList();
                if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
                    {
                    // when running behind a load balancer you can expect this header
                    var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                    // in case the IP contains a port, remove ':' and everything after
                    ipAddress = IPAddress.Parse(header.Remove(header.IndexOf(':')));
                    }
                else
                    {
                    // this will always have a value (running locally in development won't have the header)
                    ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
                    }
                ip = ipAddress.ToString();
                }
            List<KeyValuePair<string, string>> modaresAnswer = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> answer = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> jameiatanswer = new List<KeyValuePair<string, string>>();
            var id = formCollection.Where(p => p.Key == "AssessmentId").FirstOrDefault().Value;
            foreach (var item in formCollection)
                {

                if (item.Key.StartsWith("chandanswer"))
                    {
                    var Key = (item.Key).Split('_');
                    answer.Add(new KeyValuePair<string, string>(Key[1], item.Value));
                    }
                else if (item.Key.StartsWith("jameiatanswer"))
                    {
                    var Key = (item.Key).Split('_');
                    jameiatanswer.Add(new KeyValuePair<string, string>(Key[1], item.Value));

                    }
                else if (item.Key.StartsWith("modaresanswer"))
                    {
                    var Key = (item.Key).Split('_');
                    modaresAnswer.Add(new KeyValuePair<string, string>(Key[1], item.Value));

                    }
                }
            if (answer.Count() > 1)
                {
                UserAnswerInAssessmentDto model = new UserAnswerInAssessmentDto();
                if ( formCollection.Where(p => p.Key == "deccriptionAnswer").FirstOrDefault().Value!="")
                    {
                    model.deccriptionAnswers = formCollection.Where(p => p.Key == "deccriptionAnswer").FirstOrDefault().Value;
                    }
                model.answer = answer;
                model.jameiatanswer= jameiatanswer;
                model.AssessmentId = Int32.Parse(id);
                model.Ip = ip;
                model.WorkPlaceId = WorkPlaceId;
                model.username = GetOrSetBasket();
                model.modaresAnswers= modaresAnswer;
                 var result = _addUserAnswerInAssessment.Add(model);

                return Json(new ResultDto() { IsSuccess = result.IsSuccess, Message = result.Message });
                }
            return Json(new ResultDto() { IsSuccess = false, Message = "خطا در ارسال اطلاعات توسط کاربر!!!!" });

            }

        private string GetOrSetBasket()
            {

            if (_signInManager.IsSignedIn(User))
                {
                var usermyId = User.Identity.Name;
                userId = usermyId;
                return userId;
                }
            else
                {
                return SetCookiesForSurvay();

                }
            }
        private string SetCookiesForSurvay()
            {
            string survayCookieName = "VisitorId";
            if (Request.Cookies.ContainsKey(survayCookieName))
                {
                userId = Request.Cookies[survayCookieName];
                }
            if (userId != null) return userId;
            userId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true };
            cookieOptions.Expires = DateTime.Today.AddYears(2);
            Response.Cookies.Append(survayCookieName, userId, cookieOptions);

            return userId;
            }
        }
    }
