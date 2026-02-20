using MediatR;

namespace Domain.Primitives;

/// <summary>
/// Clase base para todos los eventos de dominio en el sistema.
/// Los eventos de dominio representan algo que sucedió en el dominio
/// y que otras partes del sistema necesitan conocer.
/// </summary>
/// <param name="Id">Identificador único del evento</param>
public record DomainEvent(Guid Id): INotification;
