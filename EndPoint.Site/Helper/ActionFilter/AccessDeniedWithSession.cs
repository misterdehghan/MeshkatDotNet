using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Helper.ActionFilter
{
    public class AccessDeniedWithSession : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
         
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("AccessDeniedUser") != null && context.HttpContext.Session.GetString("AccessDeniedUser") != "")
            {
                if (context.Controller is Microsoft.AspNetCore.Mvc.Controller controller)
                {
                    context.Result = controller.RedirectToAction("AccessDenied", "Account", new { area = "", message = "به علت ایجاد در خواست غیر مجاز حساب شما 20 دقیقه مسدود می باشد!!!" });
                }

            }

        }
    }
}
