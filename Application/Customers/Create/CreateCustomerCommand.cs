using ErrorOr;
using MediatR;

namespace Application.Customers.Create;

/// <summary>
/// Comando para crear un nuevo cliente en el sistema.
/// Implementa el patrón CQRS (Command Query Responsibility Segregation).
/// Este comando encapsula todos los datos necesarios para la creación de un cliente.
/// </summary>
/// <param name="Name">Nombre del cliente (requerido)</param>
/// <param name="LastName">Apellido del cliente (requerido)</param>
/// <param name="Email">Correo electrónico del cliente (requerido)</param>
/// <param name="PhoneNumber">Número de teléfono validado (requerido)</param>
/// <param name="Country">País de residencia (requerido)</param>
/// <param name="Line1">Primera línea de dirección (calle y número)</param>
/// <param name="Line2">Segunda línea de dirección (apartamento, suite, etc.)</param>
/// <param name="City">Ciudad de residencia (requerido)</param>
/// <param name="State">Estado o provincia (requerido)</param>
/// <param name="ZipCode">Código postal (requerido)</param>
/// <remarks>
/// Este comando será procesado por CreateCustomerCommandHandler.
/// Implementa IRequest&lt;Unit&gt; para indicar que no retorna valor específico,
/// solo confirma la operación completada.
/// </remarks>
public record CreateCustomerCommand(
    string Name,
    string LastName,
    string Email,
    string PhoneNumber,
    string Country,
    string Line1,
    string Line2,
    string City,
    string State,
    string ZipCode
) : IRequest<ErrorOr<Unit>>;
