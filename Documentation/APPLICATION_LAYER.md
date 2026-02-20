# 📦 **Application Layer - Capa de Aplicación**

## 🎯 **Propósito**

La capa Application es la capa intermedia entre el dominio y la infraestructura. Contiene la lógica de aplicación, casos de uso, y coordinación entre diferentes componentes del sistema.

## 🏗️ **Arquitectura**

### **Responsabilidades:**
- **Casos de Uso**: Implementación de business workflows
- **MediatR**: Manejo de comandos, queries y eventos
- **Validación**: Reglas de validación de negocio
- **DTOs**: Objetos de transferencia de datos
- **Mapeo**: Transformación entre entidades y DTOs

### **Patrones Aplicados:**
- **CQRS**: Command Query Responsibility Segregation
- **Mediator Pattern**: Desacoplamiento de handlers
- **Repository Pattern**: Abstracción de acceso a datos
- **Dependency Injection**: Inversión de control

---

## 📁 **Componentes Implementados**

### **1. DependencyInjection.cs**
```csharp
namespace Application;

/// <summary>
/// Clase estática para configurar la inyección de dependencias de la capa Application.
/// Proporciona métodos de extensión para IServiceCollection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega los servicios de la capa Application al contenedor de dependencias.
    /// Configura MediatR para manejo de comandos y eventos.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación</param>
    /// <returns>Colección de servicios configurada</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly<ApplicationAssemblyReference.Assembly>();
        });

        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

        return services;
    }
}
```

#### **🎯 Características:**
- ✅ **Método de extensión**: `AddApplication()`
- ✅ **MediatR configurado**: Para CQRS y eventos
- ✅ **Validadores registrados**: Para validación de DTOs
- ✅ **Comentarios XML**: Documentación profesional

---

### **2. ApplicationAssemblyReference.cs**
```csharp
namespace Application;

/// <summary>
/// Referencia al ensamblado de la capa Application.
/// Utilizada para registrar servicios de MediatR y validadores.
/// </summary>
public class ApplicationAssemblyReference
{
    /// <summary>
    /// Referencia estática al ensamblado actual de Application.
    /// Usada por MediatR para descubrir handlers y eventos.
    /// </summary>
    internal static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}
```

#### **🎯 Características:**
- ✅ **Referencia de ensamblado**: Para descubrimiento automático
- ✅ **Static readonly**: Thread-safe y performante
- ✅ **MediatR integration**: Registro automático de handlers
- ✅ **Comentarios XML**: Propósito claro

---

### **3. IApplicationDbContext.cs**
```csharp
namespace Application.Data;

/// <summary>
/// Interfaz de contexto de aplicación para acceso a datos.
/// Abstrae el acceso a la base de datos sin acoplar a Entity Framework.
/// Permite tener los objetos de entidad sin anidarlos a una BD específica.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Conjunto de entidades Customer para operaciones de base de datos.
    /// Proporciona acceso CRUD a la entidad Customer.
    /// </summary>
    DbSet<Customer> Customers { get; set; }
    
    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// Operación asíncrona para mejor performance.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelación de operación</param>
    /// <returns>Número de entidades afectadas</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

#### **🎯 Características:**
- ✅ **Abstracción de BD**: Desacoplado de Entity Framework
- ✅ **DbSet<Customer>**: Acceso a entidades de dominio
- ✅ **SaveChangesAsync**: Persistencia asíncrona
- ✅ **CancellationToken**: Soporte para cancelación
- ✅ **Comentarios XML**: Documentación completa

---

## 🔄 **Flujo de Trabajo**

### **Secuencia de Operaciones:**
```
1. Web.API (Controller)
   ↓
2. Application Layer (Command/Query)
   ↓
3. Domain Layer (Entidades y Lógica)
   ↓
4. Infrastructure Layer (Persistencia)
```

### **Ejemplo de Uso:**
```csharp
// En Web.API Program.cs
builder.Services.AddApplication();

// En Controller
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
```

---

## 📦 **Dependencias**

### **Paquetes NuGet:**
- ✅ **Microsoft.EntityFrameworkCore**: v11.0.0-preview.1.26104.118
- ✅ **MediatR**: Para CQRS y eventos
- ✅ **FluentValidation**: Para validación

### **Referencias de Proyecto:**
- ✅ **Domain**: Entidades y lógica de negocio
- ⏳ **Infrastructure**: Implementación de repositorios

---

## 🚀 **Próximos Pasos**

### **Componentes por Implementar:**
1. **Commands**: CreateCustomerCommand, UpdateCustomerCommand
2. **Queries**: GetCustomerByIdQuery, GetAllCustomersQuery
3. **Handlers**: Implementación de comandos y queries
4. **DTOs**: CustomerDto, CreateCustomerRequest
5. **Validators**: Reglas de validación para DTOs
6. **Mappers**: Perfiles de AutoMapper

### **Mejoras Sugeridas:**
- **Unit Tests**: Pruebas para handlers y validadores
- **Logging**: Registro de operaciones
- **Exception Handling**: Manejo centralizado de errores
- **Performance**: Caching y optimización

---

## 📊 **Estado Actual**

### **✅ Completado:**
- DependencyInjection configurado
- ApplicationAssemblyReference implementado
- IApplicationDbContext definido
- Comentarios XML profesionales
- Documentación actualizada

### **⏳ Pendiente:**
- Commands y Queries
- Handlers implementation
- DTOs y Validators
- Integration tests

---

## 🎯 **Buenas Prácticas Aplicadas**

- ✅ **Clean Architecture**: Separación clara de responsabilidades
- ✅ **Dependency Injection**: Inversión de control
- ✅ **Async/Await**: Operaciones asíncronas
- ✅ **XML Documentation**: Código auto-documentado
- ✅ **SOLID Principles**: Diseño mantenible y escalable

---

*Última actualización: 20/02/2026*
*Versión: 1.0.0*
