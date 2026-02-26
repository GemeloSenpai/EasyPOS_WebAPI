using Domain.Customers;
using Domain.Primitives;
using Domain.ValueObjects;
using MediatR;
using ErrorOr;

namespace Application.Customers.Create;

/// <summary>
/// Handler para procesar el comando CreateCustomerCommand.
/// Implementa el patrón CQRS como parte de la capa de aplicación.
/// Orquesta la creación de clientes coordinando dominio e infraestructura.
/// </summary>
/// <remarks>
/// Este handler es responsable de:
/// 1. Validar y crear Value Objects (PhoneNumber, Address)
/// 2. Crear la entidad Customer del dominio
/// 3. Persistir la entidad mediante el repositorio
/// 4. Confirmar la transacción mediante Unit of Work
/// </remarks>
internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Unit>>
{
    /// <summary>
    /// Repositorio de clientes para operaciones de persistencia.
    /// Inyectado mediante Dependency Injection.
    /// </summary>
    private readonly ICustomerRepository _customerRepository;
    
    /// <summary>
    /// Unidad de trabajo para manejar transacciones.
    /// Asegura atomicidad en las operaciones de base de datos.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructor del handler con inyección de dependencias.
    /// </summary>
    /// <param name="customerRepository">Repositorio de clientes</param>
    /// <param name="unitOfWork">Unidad de trabajo</param>
    /// <exception cref="ArgumentNullException">Lanzado si algún parámetro es nulo</exception>
    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Maneja la ejecución del comando CreateCustomerCommand.
    /// Implementa la lógica de negocio para crear un nuevo cliente.
    /// Retorna ErrorOr&lt;Unit&gt; para manejo tipado de errores y éxitos.
    /// </summary>
    /// <param name="command">Comando con datos del cliente a crear</param>
    /// <param name="cancellationToken">Token para cancelación de operación</param>
    /// <returns>ErrorOr&lt;Unit&gt; con resultado de la operación</returns>
    /// <remarks>
    /// Posibles retornos:
    /// - Success(Unit.Value): Cliente creado exitosamente
    /// - Validation.Error: Datos de entrada inválidos
    /// - Error.Failure: Error interno del sistema
    /// </remarks>
    
    public async Task<ErrorOr<Unit>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // Validar y crear PhoneNumber Value Object
            if(PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
            {
                return Error.Validation("Customer.PhoneNumber", "El número de teléfono no es válido");
            }
            
            // Validar y crear Address Value Object
            if(Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode) is not Address address)
            {
                return Error.Validation("Customer.Address", "La dirección no es válida");
            }

            // Crear entidad Customer del dominio con ID único
            var customer = new Customer(
                new CustomerId(Guid.NewGuid()),
                command.Name,
                command.LastName,
                command.Email,
                phoneNumber,
                address,
                true
            );
            
            // Persistir la entidad mediante el repositorio
            await _customerRepository.Add(customer);

            // Confirmar la transacción
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            // Retornar Unit para indicar éxito
            return Unit.Value;
        }
        catch (Exception ex)
        {
            // Capturar excepciones inesperadas y convertirlas a ErrorOr
            return Error.Failure("CreateCustomer.Failure", $"Error interno al crear cliente: {ex.Message}");
        }
    }
}
