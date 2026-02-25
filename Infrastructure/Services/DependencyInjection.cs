using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Data;
using Domain.Customers;
using Domain.Primitives;
using Infrastructure.Persistence;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

/// <summary>
/// Configuración de inyección de dependencias para la capa de Infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de Infrastructure en el contenedor de DI.
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Colección de servicios configurada</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Registrar ApplicationDbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        // Registrar IApplicationDbContext
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Registrar Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Registrar repositorios
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
