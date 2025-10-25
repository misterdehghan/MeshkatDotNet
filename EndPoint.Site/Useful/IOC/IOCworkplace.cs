using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azmoon.Application.Interfaces.Group;
using Azmoon.Application.Service.Group.Command;
using Azmoon.Application.Interfaces.WorkPlace;
using Azmoon.Application.Service.WorkPlace.Command;
using Azmoon.Application.Service.WorkPlace.Query;

namespace EndPoint.Site.Useful.IOC
{
    public static class IOCworkplace
    {
        public static IServiceCollection AddInjectWorkPlaceAplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateGroup, CreateGroup>();
            services.AddScoped<ICreateWorkPlace, CreateWorkPlace>();
            services.AddScoped<IGetWorkPlace, GetWorkPlace>();
            services.AddScoped<IGetWorkPlaceSelectListItem, GetWorkPlaceSelectListItem>();
            services.AddScoped<IGetChildrenWorkPlace, GetChildrenWorkPlacees>();
            return services;
        }
    }
}
