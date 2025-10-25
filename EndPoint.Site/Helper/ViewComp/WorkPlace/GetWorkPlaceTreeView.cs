using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Azmoon.Application.Interfaces.WorkPlace;

namespace EndPoint.Site.Helper.ViewComp.WorkPlace
{
    public class GetWorkPlaceTreeView : ViewComponent
    {
        private readonly IGetWorkPlace _getWorkPlace;

        public GetWorkPlaceTreeView(IGetWorkPlace getWorkPlace)
        {
            _getWorkPlace = getWorkPlace;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = _getWorkPlace.GetTreeView();
            var viewName = $"~/Views/Shared/Components/WorkPlace/{this.ViewComponentContext.ViewComponentDescriptor.ShortName}.cshtml";

            return await Task.FromResult((IViewComponentResult)View(viewName ,model.Data));
        }
    }
}
