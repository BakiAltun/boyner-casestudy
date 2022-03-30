using Boyner.CaseStudy.ApplicationCore.Validations;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OBilet.CaseStudy.Infrastructure.Application.Validations;
using System.Reflection;

namespace Boyner.CaseStudy.Infrastructure.MediatR
{
    public static class MediatRExtennsions
    {

        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(MediatRExtennsions));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(new[] { typeof(GetJourneyQueryValidator).GetTypeInfo().Assembly });
            });
            return services;
        } 

    }
}
