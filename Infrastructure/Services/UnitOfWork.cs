using Application.Data;
using Domain.Primitives;
using Infrastructure.Persistence;

namespace Infrastructure.Services;

/// <summary>
/// Implementación del patrón Unit of Work.
/// Maneja transacciones y asegura consistencia en las operaciones de base de datos.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor del Unit of Work.
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Persiste todos los cambios pendientes en la base de datos.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelación de la operación</param>
    /// <returns>Número de entidades afectadas por la operación</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
