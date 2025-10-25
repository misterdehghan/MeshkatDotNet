using Azmoon.Application.Service.Survaeis.Questiones;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Helper.ViewComp.Survay
{
    public class GetQuestionBySurvayId :ViewComponent
    {
    

        public async Task<IViewComponentResult> InvokeAsync(List<GetQuestionSurvayViewModel> Questiones)
        {

            var viewName = $"~/Areas/Admin/Views/Shared/Components/Survay/{this.ViewComponentContext.ViewComponentDescriptor.ShortName}.cshtml";

            return await Task.FromResult((IViewComponentResult)View(viewName, Questiones));
        }
    }
}
