using Azmoon.Application.Interfaces.Answer;
using Azmoon.Application.Service.Answer.Command;
using Azmoon.Application.Service.Answer.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCAnswersInject
    {
        public static IServiceCollection AddIOCAnswersInject(this IServiceCollection services)
        {
            services.AddScoped<IAddAnswer,AddAnswer >();
            services.AddScoped<IGetAnswer, GetAnswer>();
            return services;
        }
    }
}
