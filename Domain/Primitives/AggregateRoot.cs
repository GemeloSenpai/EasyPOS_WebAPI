namespace Domain.Primitives;

/// <summary>
/// Clase base abstracta para todas las raíces de agregado (Aggregate Root).
/// Un Aggregate Root es una entidad que controla el acceso a un cluster
/// de objetos relacionados que se tratan como una unidad.
/// </summary>
public abstract class AggregateRoot
{
    /// <summary>
    /// Lista privada que almacena todos los eventos de dominio
    /// que han ocurrido en este agregado pero aún no se han procesado.
    /// </summary>
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Obtiene la colección de eventos de dominio pendientes de procesar.
    /// Usado por la infraestructura para publicar los eventos.
    /// </summary>
    /// <returns>Colección de eventos de dominio</returns>
    public ICollection<DomainEvent> GetDomainEvents() => _domainEvents;

    /// <summary>
    /// Agrega un nuevo evento de dominio a la lista de eventos pendientes.
    /// Este método es protegido para que solo las clases hijas puedan disparar eventos.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio a agregar</param>
    protected void Raise(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
