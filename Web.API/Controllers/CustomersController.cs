using Application.Customers.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

/// <summary>
/// Controlador para gestionar operaciones relacionadas con clientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor del controlador de clientes.
    /// </summary>
    /// <param name="mediator">Mediador para enviar comandos y queries</param>
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crea un nuevo cliente en el sistema.
    /// </summary>
    /// <param name="command">Datos del cliente a crear</param>
    /// <param name="cancellationToken">Token para cancelación de operación</param>
    /// <returns>Respuesta de la operación</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(
        [FromBody] CreateCustomerCommand command,
        CancellationToken cancellationToken = default)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok(new { message = "Cliente creado exitosamente" });
    }
}
