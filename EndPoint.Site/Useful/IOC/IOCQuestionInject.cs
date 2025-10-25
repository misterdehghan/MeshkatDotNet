using Azmoon.Application.Interfaces.Question;
using Azmoon.Application.Service.Question.Command;
using Azmoon.Application.Service.Question.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IOCQuestionInject
    {
        public static IServiceCollection AddIOCQuestionInject(this IServiceCollection services)
        {
          services.AddScoped<IAddQuestion, AddQuestion>();
          services.AddScoped<IGetQuestion,GetQuestion >();
            return services;
        }
    }
}
