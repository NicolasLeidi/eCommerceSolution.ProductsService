using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Products.BusinessLogic.ServiceContracts;
using Products.BusinessLogic.Services;
using Products.BusinessLogic.Validators;

namespace Products.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddTransient<IProductService, ProductService>();
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductUpdateRequestValidator>();
        return services;
    }
}