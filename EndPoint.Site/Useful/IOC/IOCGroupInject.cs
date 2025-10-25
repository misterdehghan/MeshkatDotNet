using Azmoon.Application.Interfaces.Group;
using Azmoon.Application.Service.Group.Command;
using Azmoon.Application.Service.Group.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCGroupInject
    {
        public static IServiceCollection AddIOCGroupInject(this IServiceCollection services)
        {
            services.AddScoped <IGetGroup, GetGroup> (); 
            services.AddScoped <IGetGroupSelectListItem, GetGroupSelectListItem> ();     
            services.AddScoped <IGetChildrenGroup, GetChildrenGroup> ();                 
            services.AddScoped <IAddGroupInUser, AddGroupInUser> ();                     
            services.AddScoped <IDeleteGroupAccess, DeleteGroupAccess> ();               

            return services;
        }
    }
}
