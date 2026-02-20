# 🏗️ Primitivas de Dominio - EasyPOS

## 📋 Visión General

Las primitivas de dominio son las clases base fundamentales que implementan los patrones de Domain-Driven Design (DDD) en nuestro proyecto EasyPOS. Estas clases proporcionan la infraestructura necesaria para manejar entidades, eventos y transacciones de manera consistente.

---

## 📦 Componentes Implementados

### **1. DomainEvent.cs**
```csharp
using MediatR;

namespace Domain.Primitives;

/// <summary>
/// Clase base para todos los eventos de dominio en el sistema.
/// Los eventos de dominio representan algo que sucedió en el dominio
/// y que otras partes del sistema necesitan conocer.
/// </summary>
/// <param name="Id">Identificador único del evento</param>
public record DomainEvent(Guid Id): INotification;
```

#### **🎯 Propósito:**
- **Base para eventos**: Todos los eventos de dominio heredan de aquí
- **Integración MediatR**: Facilita el manejo asíncrono de eventos
- **Inmutabilidad**: Los records son inmutables por naturaleza

#### **🔹 Características:**
- **Guid Id**: Identificador único para cada evento
- **INotification**: Interfaz de MediatR para notificaciones
- **Record Type**: Clase inmutable por defecto

#### **💡 Ejemplos de Uso:**
```csharp
// Eventos específicos que heredan de DomainEvent
public record ProductCreated(Guid Id, string Name, decimal Price) : DomainEvent(Id);
public record OrderCompleted(Guid Id, Guid CustomerId, decimal Total) : DomainEvent(Id);
public record CustomerUpdated(Guid Id, string Email, string Phone) : DomainEvent(Id);
```

---

### **2. AggregateRoot.cs**
```csharp
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
```

#### **🎯 Propósito:**
- **Raíz de agregado**: Controla el acceso a entidades relacionadas
- **Manejo de eventos**: Almacena eventos del dominio
- **Base abstracta**: Todas las entidades principales heredan de aquí

#### **🔹 Componentes:**
- **_domainEvents**: Lista privada de eventos pendientes
- **GetDomainEvents()**: Método público para acceder a eventos
- **Raise()**: Método protegido para agregar eventos

#### **💡 Ejemplo de Uso:**
```csharp
public class Product : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public Product(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
        
        // Disparar evento de creación
        Raise(new ProductCreated(Id, Name, Price));
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0) 
            throw new ArgumentException("Price must be positive");
        
        Price = newPrice;
        Raise(new ProductPriceUpdated(Id, newPrice));
    }
}
```

---

### **3. IUnitOfWork.cs**
```csharp
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
```

#### **🎯 Propósito:**
- **Unidad de trabajo**: Maneja transacciones de base de datos
- **Abstracción**: Desacopla el dominio de la infraestructura
- **Transaccionalidad**: Asegura consistencia atómica

#### **🔹 Características:**
- **SaveChangesAsync()**: Método para persistir cambios
- **CancellationToken**: Soporte para cancelación de operaciones
- **Task<int>**: Retorna número de entidades afectadas

#### **💡 Implementación Típica:**
```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly IPublisher _mediator;

    public UnitOfWork(DbContext context, IPublisher mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Procesar eventos de dominio antes de guardar
        await DispatchDomainEvents(cancellationToken);
        
        return await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEvents(CancellationToken cancellationToken)
    {
        // Obtener todos los agregados con eventos pendientes
        var domainEntities = _context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.GetDomainEvents().Any())
            .ToList();

        // Publicar cada evento con MediatR
        foreach (var entity in domainEntities)
        {
            var events = entity.Entity.GetDomainEvents().ToList();
            
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
            
            entity.Entity.ClearDomainEvents(); // Limpiar eventos después de procesar
        }
    }
}
```

---

## 🔄 **Flujo de Trabajo Completo**

### **Secuencia de Operaciones:**
```
1. Entidad (hereda de AggregateRoot)
   ↓
2. Ejecuta acción de negocio
   ↓
3. Dispara evento con Raise()
   ↓
4. UnitOfWork procesa eventos
   ↓
5. MediatR distribuye a handlers
   ↓
6. Persistencia con SaveChangesAsync()
```

### **Ejemplo Completo:**
```csharp
// 1. Entidad de dominio
public class Order : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal Total { get; private set; }
    public OrderStatus Status { get; private set; }

    public void Complete()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order cannot be completed");
            
        Status = OrderStatus.Completed;
        Raise(new OrderCompleted(Id, CustomerId, Total));
    }
}

// 2. Evento específico
public record OrderCompleted(Guid Id, Guid CustomerId, decimal Total) 
    : DomainEvent(Id);

// 3. Handler del evento
public class OrderCompletedHandler : INotificationHandler<OrderCompleted>
{
    private readonly IEmailService _emailService;
    private readonly IInventoryService _inventoryService;

    public OrderCompletedHandler(IEmailService emailService, IInventoryService inventoryService)
    {
        _emailService = emailService;
        _inventoryService = inventoryService;
    }

    public async Task Handle(OrderCompleted notification, CancellationToken cancellationToken)
    {
        // Enviar email de confirmación
        await _emailService.SendOrderConfirmation(notification.CustomerId);
        
        // Actualizar inventario
        await _inventoryService.UpdateInventoryForOrder(notification.Id);
        
        await Task.CompletedTask;
    }
}
```

---

## 🎯 **Ventajas de esta Arquitectura**

### **✅ Clean Architecture:**
- **Dominio puro**: Sin dependencias externas
- **Eventos asíncronos**: Mejor rendimiento
- **Desacoplamiento**: Componentes independientes

### **✅ Domain-Driven Design:**
- **Aggregate Root**: Control de acceso al agregado
- **Domain Events**: Acciones secundarias desacopladas
- **Unit of Work**: Transacciones consistentes

### **✅ Buenas Prácticas:**
- **Records**: Inmutabilidad para eventos
- **MediatR**: Patrones CQRS y Mediator
- **Async/Await**: Operaciones no bloqueantes
- **Documentación XML**: IntelliSense y mantenibilidad

---

## 📝 **Consideraciones de Implementación**

### **Dependencias Requeridas:**
```bash
# Para Domain layer
dotnet add Domain package MediatR

# Para Infrastructure layer (implementación)
dotnet add Infrastructure package Microsoft.EntityFrameworkCore
dotnet add Infrastructure package MediatR.Extensions.Microsoft.DependencyInjection
```

### **Buenas Prácticas:**
1. **Eventos Inmutables**: Usar records para eventos
2. **Nombres Descriptivos**: Eventos con nombres claros (Created, Updated, Deleted)
3. **Handlers Asíncronos**: Siempre usar async/await
4. **Validaciones**: Validar antes de disparar eventos
5. **Logging**: Registrar eventos importantes

### **Errores Comunes a Evitar:**
1. **Eventos Mutables**: Nunca modificar eventos después de creados
2. **Lógica en Handlers**: Mantener lógica de negocio en el dominio
3. **Eventos Síncronos**: Siempre procesar eventos de forma asíncrona
4. **Excepciones en Handlers**: Manejar excepciones apropiadamente

---

## 🚀 **Próximos Pasos**

1. **Crear Entidades Concretas**: Product, Customer, Order, etc.
2. **Implementar Repositorios**: Usando IUnitOfWork
3. **Crear Handlers Específicos**: Para cada tipo de evento
4. **Configurar Inyección**: En capa Application
5. **Agregar Validaciones**: FluentValidation
6. **Implementar Testing**: Unit tests para eventos y handlers

---

## 📚 **Recursos Adicionales**

- [Domain-Driven Design - Eric Evans](https://amzn.to/3DdQz9V)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Clean Architecture - Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Entity Framework Core](https://docs.microsoft.com/es-es/ef/core/)

---

*Última actualización: 20/02/2026*
