using Application.Data;
using Domain.Customers;
using Domain.Primitives;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de clientes usando Entity Framework.
/// Proporciona operaciones CRUD para la entidad Customer.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor del repositorio de clientes.
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Cliente encontrado o null</returns>
    public async Task<Customer?> GetByIdAsync(CustomerId id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Agrega un nuevo cliente al repositorio.
    /// </summary>
    /// <param name="customer">Cliente a agregar</param>
    /// <returns>Tarea asíncrona</returns>
    public async Task Add(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }
}
