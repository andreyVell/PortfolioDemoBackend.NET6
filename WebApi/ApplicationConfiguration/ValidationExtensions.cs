using FluentValidation;
using FluentValidation.AspNetCore;
using WebApi.DTOs;

namespace WebApi.ApplicationConfiguration
{
    public static class ValidationExtensions
    {
        public static void AddCustomValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<IValidatorRegistrator>();
        }
    }
}
