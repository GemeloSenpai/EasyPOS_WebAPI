using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

/// <summary>
/// Interfaz de contexto de aplicación para acceso a datos.
/// Abstrae el acceso a la base de datos sin acoplar a Entity Framework.
/// Permite tener los objetos de entidad sin anidarlos a una BD específica.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Conjunto de entidades Customer para operaciones de base de datos.
    /// Proporciona acceso CRUD a la entidad Customer.
    /// </summary>
    DbSet<Customer> Customers { get; set; }
    
    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// Operación asíncrona para mejor performance.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelación de operación</param>
    /// <returns>Número de entidades afectadas</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}