using Azmoon.Application.Interfaces.Quiz;
using Azmoon.Application.Service.Quiz;
using Azmoon.Application.Service.Quiz.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCQuizInject
    {
        public static IServiceCollection AddIOCQuizInject(this IServiceCollection services)
        {
            services.AddScoped<IAddQuiz,AddQuiz >();
            services.AddScoped<IGetQuiz, GetQuiz>();
            services.AddScoped<IGetQuizForStudendt, GetQuizForStudendt>();
            return services;
        }
    }
}
