using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Customers;

/// <summary>
/// Entidad de dominio que representa un cliente en el sistema.
/// Esta entidad representa la lógica de negocio, no la tabla de la base de datos.
/// Hereda de AggregateRoot para manejar eventos de dominio.
/// </summary>
public sealed class Customer : AggregateRoot
{
    /// <summary>
    /// Constructor principal para crear una nueva instancia de Customer.
    /// Valida y asigna todos los valores requeridos.
    /// </summary>
    /// <param name="id">Identificador único del cliente</param>
    /// <param name="name">Nombre del cliente</param>
    /// <param name="lastName">Apellido del cliente</param>
    /// <param name="email">Correo electrónico del cliente</param>
    /// <param name="phoneNumber">Número de teléfono validado</param>
    /// <param name="address">Dirección del cliente</param>
    /// <param name="active">Estado activo del cliente</param>
    public Customer(CustomerId id, string name, string lastName, string email, PhoneNumber phoneNumber, Address address, bool active)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        this.email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Active = active;
    }

    /// <summary>
    /// Constructor privado vacío requerido por Entity Framework.
    /// No debe usarse para creación manual de instancias.
    /// </summary>
    private Customer()
    {
        
    }

    /// <summary>
    /// Identificador único fuertemente tipado del cliente.
    /// Es inmutable para asegurar consistencia.
    /// </summary>
    public CustomerId Id { get; private set; }

    /// <summary>
    /// Nombre del cliente.
    /// Propiedad privada para asegurar encapsulamiento.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Apellido del cliente.
    /// Propiedad pública para permitir modificación (diseño actual).
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del cliente (propiedad calculada).
    /// Combina nombre y apellido para facilitar consultas.
    /// </summary>
    public string FullName => $"{Name} {LastName}";

    /// <summary>
    /// Correo electrónico del cliente.
    /// Propiedad privada para asegurar encapsulamiento.
    /// </summary>
    public string email { get; private set; } = string.Empty;

    /// <summary>
    /// Número de teléfono validado del cliente.
    /// Usa ValueObject PhoneNumber para asegurar formato correcto.
    /// </summary>
    public PhoneNumber PhoneNumber { get; private set; }

    /// <summary>
    /// Dirección del cliente.
    /// ValueObject Address para representar dirección completa.
    /// </summary>
    public Address Address { get; private set; }

    /// <summary>
    /// Estado activo del cliente.
    /// Determina si el cliente puede realizar operaciones.
    /// </summary>
    public bool Active { get; private set; }
}
