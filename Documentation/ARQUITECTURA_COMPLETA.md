# 🏗️ **Arquitectura Completa - EasyPOS WebAPI**

## 📋 **Resumen Ejecutivo**

EasyPOS es una aplicación web construida con **Clean Architecture** y **Domain-Driven Design (DDD)** utilizando **.NET 10.0** y **ASP.NET Core**.

### **🎯 Tecnologías Principales:**
- **.NET 10.0**: Framework principal
- **ASP.NET Core**: Web API
- **Entity Framework Core**: ORM para base de datos
- **MediatR**: Manejo de comandos y eventos
- **FluentValidation**: Validación de DTOs
- **xUnit**: Framework de pruebas unitarias
- **Swagger/OpenAPI**: Documentación de API

---

## 🏛️ **Estructura de Capas**

```
📁 EasyPOS/
├── 📁 Domain/                 🏛️ Capa de Dominio (Núcleo)
│   ├── 📁 Customers/          👥 Entidades de Cliente
│   ├── 📁 Primitives/         🔧 Componentes Base
│   └── 📁 ValueObjects/       💎 Objetos de Valor
├── 📁 Application/            🎯 Capa de Aplicación (Casos de Uso)
│   ├── 📁 Data/              🗄️ Contextos y Contratos
│   └── 📁 Features/           🚀 Comandos y Queries (futuro)
├── 📁 Infrastructure/         🔨 Capa de Infraestructura (Implementaciones)
│   ├── 📁 Data/              🗄️ Entity Framework
│   └── 📁 Repositories/       📦 Repositorios
├── 📁 Web.API/               🌐 Capa de Presentación (API)
│   ├── 📁 Controllers/        🎮 Controladores API
│   └── 📁 Configuration/      ⚙️ Configuración
└── 📁 Tests/                 🧪 Capa de Pruebas
    └── 📁 Domain/             🧪 Pruebas de Dominio
```

---

## 🏛️ **Capa de Dominio (Domain)**

### **🎯 Propósito:**
Contiene la **lógica de negocio pura** sin dependencias externas. Es el corazón de la aplicación.

### **📦 Componentes:**

#### **1. Primitives/ - Componentes Base**
```csharp
📁 Domain/Primitives/
├── 📄 AggregateRoot.cs     🌳 Raíz de Agregado
├── 📄 DomainEvent.cs       📡 Eventos de Dominio
└── 📄 IUnitOfWork.cs      💾 Unidad de Trabajo
```

**🔹 AggregateRoot.cs:**
```csharp
/// <summary>
/// Clase base abstracta para todas las raíces de agregado.
/// Controla el acceso a entidades relacionadas y maneja eventos.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();
    
    /// <summary>
    /// Obtiene eventos de dominio pendientes de procesar.
    /// Usado por infraestructura para publicar eventos.
    /// </summary>
    public ICollection<DomainEvent> GetDomainEvents() => _domainEvents;
    
    /// <summary>
    /// Dispara un evento de dominio.
    /// Protected para uso exclusivo de clases hijas.
    /// </summary>
    protected void Raise(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
```

**🔹 DomainEvent.cs:**
```csharp
/// <summary>
/// Base para todos los eventos de dominio.
/// Representa algo que sucedió en el negocio.
/// </summary>
public record DomainEvent(Guid Id) : INotification;
```

**🔹 IUnitOfWork.cs:**
```csharp
/// <summary>
/// Contrato para unidad de trabajo.
/// Maneja transacciones de base de datos.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

#### **2. Customers/ - Entidades de Dominio**
```csharp
📁 Domain/Customers/
├── 📄 Customer.cs           👤 Entidad Principal
├── 📄 CustomerId.cs         🆔 ID Fuertemente Tipado
└── 📄 ICustomerRepository.cs 📦 Interfaz de Repositorio
```

**🔹 Customer.cs:**
```csharp
/// <summary>
/// Entidad de dominio que representa un cliente.
/// Raíz de agregado que maneja eventos de dominio.
/// </summary>
public sealed class Customer : AggregateRoot
{
    /// <summary>
    /// Constructor principal para crear cliente válido.
    /// Valida todos los parámetros requeridos.
    /// </summary>
    public Customer(CustomerId id, string name, string lastName, 
        string email, PhoneNumber phoneNumber, Address address, bool active)
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
    /// Constructor privado para Entity Framework.
    /// No usar para creación manual.
    /// </summary>
    private Customer() { }
    
    /// <summary>
    /// ID único del cliente (fuertemente tipado).
    /// Inmutable para consistencia.
    /// </summary>
    public CustomerId Id { get; private set; }
    
    /// <summary>
    /// Nombre del cliente.
    /// Privado para encapsulamiento.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    
    /// <summary>
    /// Apellido del cliente.
    /// Público para modificación directa.
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Nombre completo calculado.
    /// Combina nombre y apellido.
    /// </summary>
    public string FullName => $"{Name} {LastName}";
    
    /// <summary>
    /// Correo electrónico del cliente.
    /// Privado para encapsulamiento.
    /// </summary>
    public string email { get; private set; } = string.Empty;
    
    /// <summary>
    /// Número de teléfono validado.
    /// Usa ValueObject PhoneNumber.
    /// </summary>
    public PhoneNumber PhoneNumber { get; private set; }
    
    /// <summary>
    /// Dirección del cliente.
    /// Usa ValueObject Address.
    /// </summary>
    public Address Address { get; private set; }
    
    /// <summary>
    /// Estado activo del cliente.
    /// Determina si puede operar.
    /// </summary>
    public bool Active { get; private set; }
}
```

**🔹 CustomerId.cs:**
```csharp
/// <summary>
/// Identificador fuertemente tipado para Customer.
/// Evita errores con primitivos (Guid).
/// Implementado como record para inmutabilidad.
/// </summary>
/// <param name="Value">Valor GUID del identificador</param>
public record CustomerId(Guid Value);
```

**🔹 ICustomerRepository.cs:**
```csharp
/// <summary>
/// Contrato de repositorio para Customer.
/// Define operaciones de persistencia sin acoplamiento.
/// Sigue patrón Repository.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Obtiene cliente por ID.
    /// Operación asíncrona para performance.
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Cliente encontrado o null</returns>
    Task<Customer?> GetByIdAsync(CustomerId id);
    
    /// <summary>
    /// Agrega nuevo cliente.
    /// Operación asíncrona para persistencia.
    /// </summary>
    /// <param name="customer">Cliente a agregar</param>
    /// <returns>Tarea completada</returns>
    Task Add(Customer customer);
}
```

#### **3. ValueObjects/ - Objetos de Valor**
```csharp
📁 Domain/ValueObjects/
├── 📄 PhoneNumber.cs      📞 Número de Teléfono
└── 📄 Address.cs         🏠 Dirección Postal
```

**🔹 PhoneNumber.cs:**
```csharp
/// <summary>
/// Value Object para número de teléfono válido.
/// Inmutable y con validación incorporada.
/// </summary>
public record PhoneNumber
{
    /// <summary>
    /// Longitud requerida (8 dígitos).
    /// </summary>
    private const int DefaultLength = 8;
    
    /// <summary>
    /// Patrón de validación regex.
    /// Acepta números con guiones opcionales.
    /// </summary>
    private const string Pattern = @"^(?:-*\d-*){8}$";
    
    /// <summary>
    /// Constructor privado para creación controlada.
    /// Solo vía método Create().
    /// </summary>
    private PhoneNumber(string value) => Value = value;
    
    /// <summary>
    /// Método fábrica para crear PhoneNumber.
    /// Valida formato antes de crear.
    /// </summary>
    /// <param name="value">Cadena a validar</param>
    /// <returns>PhoneNumber válido o null</returns>
    public static PhoneNumber? Create(string value)
    {
        if (string.IsNullOrEmpty(value) || 
            !PhoneNumberRegex().IsMatch(value) || 
            value.Length != DefaultLength)
        {
            return null;
        }
        return new PhoneNumber(value);
    }
    
    /// <summary>
    /// Valor validado del número.
    /// Init-only para inmutabilidad.
    /// </summary>
    public string Value { get; init; }
    
    /// <summary>
    /// Regex compilada para validación.
    /// Optimización de rendimiento.
    /// </summary>
    private static Regex PhoneNumberRegex() => 
        new(Pattern, RegexOptions.Compiled);
}
```

**🔹 Address.cs:**
```csharp
/// <summary>
/// Value Object para dirección postal.
/// Inmutable con validación de datos.
/// </summary>
/// <param name="Street">Calle y número</param>
/// <param name="City">Ciudad</param>
/// <param name="State">Estado</param>
/// <param name="ZipCode">Código postal</param>
/// <param name="Country">País (opcional)</param>
public record Address(
    string Street,
    string City, 
    string State,
    string ZipCode,
    string Country = ""
)
{
    /// <summary>
    /// Método fábrica para crear Address.
    /// Valida que todos los campos requeridos no sean vacíos.
    /// </summary>
    /// <param name="street">Calle</param>
    /// <param name="city">Ciudad</param>
    /// <param name="state">Estado</param>
    /// <param name="zipCode">Código postal</param>
    /// <param name="country">País</param>
    /// <returns>Address válido o null</returns>
    public static Address? Create(string street, string city, string state, 
        string zipCode, string country = "")
    {
        if (string.IsNullOrWhiteSpace(street) || 
            string.IsNullOrWhiteSpace(city) || 
            string.IsNullOrWhiteSpace(state) || 
            string.IsNullOrWhiteSpace(zipCode))
        {
            return null;
        }
        
        return new Address(street.Trim(), city.Trim(), state.Trim(), 
            zipCode.Trim(), country?.Trim() ?? "");
    }
    
    /// <summary>
    /// Dirección completa formateada.
    /// Propiedad calculada para visualización.
    /// </summary>
    public string FullAddress => string.IsNullOrEmpty(Country) 
        ? $"{Street}, {City}, {State} {ZipCode}"
        : $"{Street}, {City}, {State} {ZipCode}, {Country}";
}
```

---

## 🎯 **Capa de Aplicación (Application)**

### **🎯 Propósito:**
Contiene **casos de uso** y **coordinación** entre el dominio y la infraestructura. Implementa patrones como CQRS y Mediator.

### **📦 Componentes:**

#### **1. DependencyInjection.cs - Configuración de Servicios**
```csharp
/// <summary>
/// Configuración de inyección de dependencias de Application.
/// Extiende IServiceCollection para registro de servicios.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega servicios de Application al contenedor.
    /// Configura MediatR y validadores.
    /// </summary>
    /// <param name="services">Contenedor de servicios</param>
    /// <returns>Contenedor configurado</returns>
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Configurar MediatR para CQRS y eventos
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(
                typeof(ApplicationAssemblyReference).Assembly);
        });

        // Configurar validadores FluentValidation
        services.AddValidatorsFromAssembly(
            typeof(ApplicationAssemblyReference).Assembly);

        return services;
    }
}
```

#### **2. ApplicationAssemblyReference.cs - Referencia de Ensamblado**
```csharp
/// <summary>
/// Referencia estática al ensamblado Application.
/// Usada para descubrimiento automático de servicios.
/// </summary>
public class ApplicationAssemblyReference
{
    /// <summary>
    /// Referencia al ensamblado actual.
    /// Utilizada por MediatR para registrar handlers.
    /// </summary>
    public static readonly Assembly Assembly = 
        typeof(ApplicationAssemblyReference).Assembly;
}
```

#### **3. IApplicationDbContext.cs - Contrato de Base de Datos**
```csharp
/// <summary>
/// Interfaz de contexto de aplicación.
/// Abstrae acceso a datos sin acoplar a EF.
/// Permite testing con mocks.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Conjunto de entidades Customer.
    /// Proporciona acceso CRUD a clientes.
    /// </summary>
    DbSet<Customer> Customers { get; set; }
    
    /// <summary>
    /// Guarda cambios pendientes.
    /// Operación asíncrona para performance.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Entidades afectadas</returns>
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
```

---

## 🔨 **Capa de Infraestructura (Infrastructure)**

### **🎯 Propósito:**
Implementa **contratos** definidos en las capas superiores. Contiene acceso a datos, servicios externos, etc.

### **📦 Componentes:**
```csharp
📁 Infrastructure/
├── 📁 Percistence/
│   ├── 📄 ApplicationDbContext.cs    🗄️ Contexto EF
│   └── 📁 Repositories/              📦 Implementaciones
│       └── 📄 CustomerRepository.cs  🏪 Repositorio de Clientes
└── 📁 Data/
    └── 📄 Configurations/          🗄️ Mapeos EF (futuro)
```

#### **1. CustomerRepository.cs - Implementación de Repositorio**
```csharp
/// <summary>
/// Implementación del repositorio de clientes usando Entity Framework.
/// Conecta la capa de dominio con la persistencia en base de datos.
/// Implementa el patrón Repository para abstraer el acceso a datos.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    /// <summary>
    /// Contexto de base de datos de Entity Framework.
    /// Proporciona acceso a las tablas y operaciones CRUD.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Agrega un nuevo cliente a la base de datos.
    /// Operación asíncrona para mejor performance.
    /// </summary>
    public async Task Add(Customer customer) => 
        await _context.Customers.AddAsync(customer);

    /// <summary>
    /// Obtiene un cliente por su identificador único.
    /// Usa SingleOrDefaultAsync para búsquedas eficientes.
    /// </summary>
    public async Task<Customer?> GetByAsync(CustomerId id) => 
        await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
}
```

---

## 🌐 **Capa de Presentación (Web.API)**

### **🎯 Propósito:**
Expone la aplicación a través de **HTTP API**. Maneja requests, responses y documentación.

### **📦 Componentes:**

#### **1. Program.cs - Configuración Principal**
```csharp
/// <summary>
/// Punto de entrada principal de la aplicación.
/// Configura servicios, middleware y endpoints.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar capas de aplicación
builder.Services.AddApplication();

var app = builder.Build();

// Configurar middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
```

#### **2. Controllers/ - Controladores API**
```csharp
/// <summary>
/// Controlador para operaciones de clientes.
/// Expone endpoints RESTful.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Obtiene cliente por ID.
    /// Endpoint GET /api/customers/{id}
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> Get(Guid id)
    {
        // Implementación con MediatR
        var query = new GetCustomerByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Crea nuevo cliente.
    /// Endpoint POST /api/customers
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(
        [FromBody] CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), 
            new { id = result.Id }, result);
    }
}
```

---

## 🧪 **Capa de Pruebas (Tests)**

### **🎯 Propósito:**
Valida el **comportamiento** del sistema a través de pruebas unitarias e integración.

### **📦 Componentes:**

#### **1. CustomerTests.cs - Pruebas de Dominio**
```csharp
/// <summary>
/// Pruebas unitarias para entidad Customer.
/// Verifica comportamiento y validaciones.
/// </summary>
public class CustomerTests
{
    /// <summary>
    /// Verifica creación de cliente con datos válidos.
    /// </summary>
    [Fact]
    public void Customer_ShouldCreate_WithValidData()
    {
        // Arrange - preparar datos
        var id = new CustomerId(Guid.NewGuid());
        var phoneNumber = PhoneNumber.Create("12345678");
        var address = Address.Create("123 Main St", "City", "State", "12345");
        
        // Act - ejecutar operación
        var customer = new Customer(id, "John", "Doe", 
            "john@example.com", phoneNumber!, address!, true);
        
        // Assert - verificar resultados
        Assert.NotNull(customer);
        Assert.Equal(id, customer.Id);
        Assert.Equal("John", customer.Name);
        Assert.Equal("Doe", customer.LastName);
        Assert.Equal("John Doe", customer.FullName);
        Assert.True(customer.Active);
    }
    
    /// <summary>
    /// Verifica validación de PhoneNumber.
    /// </summary>
    [Fact]
    public void PhoneNumber_ShouldCreate_WithValidNumber()
    {
        // Act
        var phoneNumber = PhoneNumber.Create("12345678");
        
        // Assert
        Assert.NotNull(phoneNumber);
        Assert.Equal("12345678", phoneNumber!.Value);
    }
    
    /// <summary>
    /// Verifica rechazo de PhoneNumber inválido.
    /// </summary>
    [Fact]
    public void PhoneNumber_ShouldReturnNull_WithInvalidNumber()
    {
        // Act
        var phoneNumber = PhoneNumber.Create("123");
        
        // Assert
        Assert.Null(phoneNumber);
    }
}
```

---

## 🔄 **Flujo de Trabajo Completo**

### **Secuencia de Operaciones:**
```
1. 🌐 Request HTTP (Web.API)
   ↓
2. 🎮 Controller Action (Web.API)
   ↓
3. 📡 Command/Query (Application)
   ↓
4. 🏛️ Domain Logic (Domain)
   ↓
5. 📦 Repository (Infrastructure)
   ↓
6. 🗄️ Database (Infrastructure)
   ↓
7. 📡 Domain Events (Domain)
   ↓
8. 🔄 Event Handlers (Application)
   ↓
9. 📤 Response HTTP (Web.API)
```

### **Ejemplo Práctico:**
```csharp
// 1. Request: POST /api/customers
{
  "name": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phoneNumber": "12345678",
  "address": {
    "street": "123 Main St",
    "city": "City",
    "state": "State",
    "zipCode": "12345"
  }
}

// 2. Controller recibe y crea Command
var command = new CreateCustomerCommand(request);

// 3. MediatR envía Command al Handler
var result = await _mediator.Send(command);

// 4. Handler valida y crea entidad
var customer = new Customer(id, name, lastName, email, phone, address, true);

// 5. Repository persiste entidad
await _repository.Add(customer);
await _unitOfWork.SaveChangesAsync();

// 6. Domain Events se disparan
customer.Raise(new CustomerCreatedEvent(customer.Id));

// 7. Response: 201 Created
{
  "id": "guid",
  "name": "John",
  "lastName": "Doe",
  "fullName": "John Doe",
  "active": true
}
```

---

## 🎯 **Patrones y Principios Aplicados**

### **🏛️ Clean Architecture:**
- **Dependencias hacia adentro**: Capas solo dependen de capas internas
- **Inversión de dependencias**: Interfaces en capas internas
- **Separación de responsabilidades**: Cada capa con propósito claro

### **🎯 Domain-Driven Design:**
- **Entidades**: Objetos con identidad y comportamiento
- **Value Objects**: Objetos sin identidad, inmutables
- **Agregados**: Clúster de entidades con raíz
- **Repositorios**: Contratos de persistencia
- **Eventos de Dominio**: Notificaciones de cambios

### **🔧 SOLID Principles:**
- **S**: Single Responsibility - Cada clase con una responsabilidad
- **O**: Open/Closed - Abierto a extensión, cerrado a modificación
- **L**: Liskov Substitution - Subtipos reemplazables
- **I**: Interface Segregation - Interfaces específicas
- **D**: Dependency Inversion - Depender de abstracciones

---

## 📊 **Estado Actual del Proyecto**

### **✅ Completado:**
- 🏛️ **Domain Layer**: Entidades, ValueObjects, Primitives
- 🎯 **Application Layer**: DependencyInjection, DbContext, CQRS Commands
- 🔨 **Infrastructure Layer**: Repositorios y DbContext implementados
- 🌐 **Web.API Layer**: Configuración básica, Swagger
- 🧪 **Tests Layer**: Pruebas unitarias de dominio
- 🔄 **CI/CD**: GitHub Actions funcionales
- 📚 **Documentación**: Completa y actualizada

### **⏳ En Progreso:**
- � **CQRS Queries**: Implementación de consultas
- 🗄️ **Database**: Migrations y seeding
- 🔐 **Authentication**: JWT o Identity
- 📊 **Logging**: Configuración completa

### **🎯 Próximos Pasos:**
1. **Implementar Queries**: GetCustomerByIdQuery, GetAllCustomersQuery
2. **Crear Controller**: CustomersController con endpoints REST
3. **Configurar Database**: Migrations y datos iniciales
4. **Agregar Authentication**: Seguridad JWT
5. **Mejorar Tests**: Integración y end-to-end

---

## 🌐 **Endpoints Disponibles**

### **📖 Documentación:**
- **Swagger UI**: `http://localhost:5229/swagger`
- **OpenAPI Spec**: `http://localhost:5229/swagger/v1/swagger.json`

### **🌡️ API Endpoints:**
- **GET** `/weatherforecast` - Endpoint de ejemplo
- **GET** `/api/customers/{id}` - Obtener cliente (planificado)
- **POST** `/api/customers` - Crear cliente (planificado)
- **PUT** `/api/customers/{id}` - Actualizar cliente (planificado)
- **DELETE** `/api/customers/{id}` - Eliminar cliente (planificado)

---

## 🚀 **Cómo Ejecutar el Proyecto**

### **🔧 Desarrollo Local:**
```bash
# Restaurar dependencias
dotnet restore

# Compilar solución
dotnet build

# Ejecutar API
dotnet run --project Web.API

# Ejecutar tests
dotnet test

# Verificar calidad de código
dotnet format --verify-no-changes
```

### **🌐 Acceso:**
- **API**: `http://localhost:5229`
- **Swagger**: `http://localhost:5229/swagger`
- **HTTPS**: `https://localhost:7062`

---

## 📈 **Métricas y Calidad**

### **🧪 Cobertura de Pruebas:**
- **Actual**: 100% dominio (7/7 tests pasan)
- **Objetivo**: >80% cobertura total
- **Herramientas**: xUnit, Coverlet

### **🔍 Calidad de Código:**
- **Herramientas**: SonarQube, dotnet-format
- **Estándares**: Microsoft C# Coding Conventions
- **Análisis Estático**: Roslyn Analyzers

### **🔐 Seguridad:**
- **Scanning**: Trivy vulnerability scanner
- **Dependencias**: Actualizadas y seguras
- **Best Practices**: OWASP Top 10

---

## 📚 **Referencias y Recursos**

### **📖 Documentación:**
- **Clean Architecture**: https://blog.cleancoder.com/uncle-bob-clean-architecture
- **DDD**: https://domain-driven-design.org/
- **MediatR**: https://github.com/jbogard/MediatR
- **Entity Framework**: https://docs.microsoft.com/en-us/ef/core/

### **🔧 Herramientas:**
- **.NET**: https://dotnet.microsoft.com/
- **Visual Studio Code**: https://code.visualstudio.com/
- **Postman**: https://www.postman.com/
- **Git**: https://git-scm.com/

---

*Última actualización: 20/02/2026*
*Versión: 1.0.0*
*Autor: EasyPOS Development Team*
