using Domain.Customers;
using Domain.Interfaces;

namespace Infrastructure.Percistence.Repositories;

/// <summary>
/// Implementación del repositorio de clientes usando Entity Framework.
/// Conecta la capa de dominio con la persistencia en base de datos.
/// Implementa el patrón Repository para abstraer el acceso a datos.
/// </summary>
/// <remarks>
/// Este repositorio es responsable de:
/// 1. Persistir entidades Customer en la base de datos
/// 2. Recuperar clientes por su identificador único
/// 3. Manejar operaciones asíncronas para mejor performance
/// </remarks>
public class CustomerRepository : ICustomerRepository
{
    /// <summary>
    /// Contexto de base de datos de Entity Framework.
    /// Proporciona acceso a las tablas y operaciones CRUD.
    /// Inyectado mediante Dependency Injection.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor del repositorio con inyección de dependencias.
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    /// <exception cref="ArgumentNullException">Lanzado si el contexto es nulo</exception>
    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Agrega un nuevo cliente a la base de datos.
    /// Operación asíncrona para no bloquear el hilo principal.
    /// </summary>
    /// <param name="customer">Entidad Customer a persistir</param>
    /// <returns>Task que representa la operación asíncrona</returns>
    /// <remarks>
    /// El cliente se agrega al contexto pero no se guarda inmediatamente.
    /// Es necesario llamar a SaveChangesAsync() para confirmar la transacción.
    /// </remarks>
    public async Task Add(Customer customer) => await _context.Customers.AddAsync(customer);
 
    /// <summary>
    /// Obtiene un cliente por su identificador único.
    /// Operación asíncrona con consulta optimizada.
    /// </summary>
    /// <param name="id">Identificador único del cliente</param>
    /// <returns>Customer encontrado o null si no existe</returns>
    /// <remarks>
    /// Usa SingleOrDefaultAsync para garantizar que solo se retorne un cliente
    /// o null si no se encuentra. Eficiente para búsquedas por ID.
    /// </remarks>
    public async Task<Customer?> GetByAsync(CustomerId id) => await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
}
