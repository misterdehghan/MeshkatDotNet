using Azmoon.Application.Interfaces.Result;
using Azmoon.Application.Service.Result.Cammand;
using Azmoon.Application.Service.Result.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCRezultQuizInject
    {
        public static IServiceCollection AddIOCRezultQuizInject(this IServiceCollection services)
        {
            services.AddScoped<IAddResultQuiz,AddResultQuiz >();
            services.AddScoped<IGetResultQuiz, GetResultQuiz>();
            services.AddScoped<IAutorizResultInDb, AutorizResultInDb>();
            return services;
        }
    }
}
