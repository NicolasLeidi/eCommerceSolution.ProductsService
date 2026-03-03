using Microsoft.Extensions.DependencyInjection;
using Products.DataAccess.DbContext;
using Products.DataAccess.Repositories;
using Products.DataAccess.RepositoryContracts;

namespace Products.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProductsRepository, ProductsRepository>();

        services.AddTransient<DapperDbContext>();

        return services;
    }
}