using Azmoon.Application.Interfaces.User;
using Azmoon.Application.Service.User.Command;
using Azmoon.Application.Service.User.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCUserInject
    {
        public static IServiceCollection AddIOCUserInject(this IServiceCollection services)
        {
           services.AddScoped<IGetAllUser, GetAllUser>();
           services.AddScoped<ICreateUser, CreateUser>();
           services.AddScoped<IAddRoleToUser,AddRoleToUser >();
           services.AddScoped<IFindUser, FindUser>();
           services.AddScoped<IGetUserForAddRole,GetUserForAddRole >();
           services.AddScoped<IDeleteRoleInUser, DeleteRoleInUser>();
           services.AddScoped<IGetChildrenUser,GetChildrenUser >();
           services.AddScoped<IRegisterUser, RegisterUser>();
           services.AddScoped<IUpdateProfile, UpdateProfile>();
           services.AddScoped<IForgotPasswordService,ForgotPasswordService >();
            return services;
        }
    }
}
