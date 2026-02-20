using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;

namespace Application;

/// <summary>
/// Clase estática para configurar la inyección de dependencias de la capa Application.
/// Proporciona métodos de extensión para IServiceCollection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega los servicios de la capa Application al contenedor de dependencias.
    /// Configura MediatR para manejo de comandos y eventos.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación</param>
    /// <returns>Colección de servicios configurada</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

        return services;
    }
}