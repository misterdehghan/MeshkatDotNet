using Azmoon.Application.Interfaces.Filter;
using Azmoon.Application.Service.Filter.Command;
using Azmoon.Application.Service.Filter.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCQuizFilterInject
    {
        public static IServiceCollection AddIOCQuizFilterInject(this IServiceCollection services)
        {
           services.AddScoped<IAddQuizFilter, AddQuizFilter>();
           services.AddScoped<IGetFilter,GetFilter >();
           services.AddScoped<IDeleteQuizFilter,DeleteQuizFilter >();
            return services;
        }
    }
}
