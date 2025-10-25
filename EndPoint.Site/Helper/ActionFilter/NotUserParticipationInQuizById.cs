using Azmoon.Application.Interfaces.Facad;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Helper.ActionFilter
{
    public class NotUserParticipationInQuizById :IActionFilter
    {
        private readonly IQuizFilterFacad _quizFilterFacad;

        public NotUserParticipationInQuizById(IQuizFilterFacad quizFilterFacad)
        {
            _quizFilterFacad = quizFilterFacad;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
         
            if (context.ActionArguments.TryGetValue("Id", out object value))
            {
             
                var referer = context.HttpContext.Request.Headers["Referer"].ToString();
                if (context.Controller is Microsoft.AspNetCore.Mvc.Controller controller)
                {
                    var username = context.HttpContext.User.Identity.Name;
                    var quizId = (Int64.Parse(value.ToString()));

                    if (!_quizFilterFacad.getFilter.GetNotUserParticipationInQuizById(quizId, username).IsSuccess)
                    {
                        //controller.ViewData["authorized"] = "unauthorized";
                        context.Result = controller.RedirectToAction("AccessDenied", "Account", new { area = "" });
                    }

                }
            }
         
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

       
    }
}
