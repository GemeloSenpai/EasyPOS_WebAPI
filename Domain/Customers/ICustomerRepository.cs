namespace Domain.Customers;

/// <summary>
/// Interfaz de repositorio para la entidad Customer.
/// Define el contrato para operaciones de persistencia de clientes.
/// Sigue el patrón Repository para desacoplar el dominio de la infraestructura.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Obtiene un cliente por su identificador único.
    /// Operación asíncrona para mejor performance.
    /// </summary>
    /// <param name="id">Identificador único del cliente</param>
    /// <returns>Cliente encontrado o null si no existe</returns>
    Task<Customer?> GetByIdAsync(CustomerId id);
    
    /// <summary>
    /// Agrega un nuevo cliente al repositorio.
    /// Operación asíncrona para persistencia en base de datos.
    /// </summary>
    /// <param name="customer">Entidad Customer a persistir</param>
    /// <returns>Tarea completada cuando se guarda el cliente</returns>
    Task Add(Customer customer);
}
