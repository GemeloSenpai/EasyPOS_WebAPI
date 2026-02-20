namespace Domain.Primitives;

/// <summary>
/// Interfaz que define el contrato para la Unidad de Trabajo (Unit of Work).
/// El Unit of Work maneja transacciones y asegura que todos los cambios
/// se persistan de manera atómica y consistente.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persiste todos los cambios pendientes en la base de datos.
    /// Este método debe procesar los eventos de dominio antes de guardar.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelación de la operación</param>
    /// <returns>Número de entidades afectadas por la operación</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}