using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azmoon.Application.Interfaces;
using Azmoon.Application.Service.Role.Command;
using Azmoon.Application.Interfaces.Role;
using Azmoon.Application.Service.Role.Query;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCRole
    {
        public static IServiceCollection AddInjectRoleAplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateRole, CreateRole>();
            services.AddScoped<IGetAllRolesInUser, GetAllRolesInUser>();
             services.AddScoped<IGetRoles, GetRoles>();
            return services;
        }
    }
}
