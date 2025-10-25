using Azmoon.Application.Service.Survaeis.Survayy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Helper.ViewComp.Survay
{
    public class ComponetActiveSurvayViewComponent : ViewComponent
    {
        private readonly IGeyActiveSurvey _geyActiveSurvey;

        public ComponetActiveSurvayViewComponent(IGeyActiveSurvey geyActiveSurvey)
        {
            _geyActiveSurvey = geyActiveSurvey;
        }

        public async Task<IViewComponentResult> InvokeAsync(int survaieType)
        {

            var model = _geyActiveSurvey.getSurvays(survaieType);
            var viewName = $"~/Views/Shared/Components/Survey/{this.ViewComponentContext.ViewComponentDescriptor.ShortName}.cshtml";

            return await Task.FromResult((IViewComponentResult)View(viewName, model));
        }
    }
}
