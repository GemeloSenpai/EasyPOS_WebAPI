# 🔨 **Infrastructure Layer - EasyPOS**

## 📋 **Resumen de la Capa de Infraestructura**

La capa de **Infrastructure** contiene todas las implementaciones técnicas que conectan la aplicación con el mundo exterior. Implementa los contratos definidos en las capas de dominio y aplicación.

---

## 🏗️ **Estructura de la Capa**

```
📁 Infrastructure/
├── 📁 Percistence/
│   ├── 📄 ApplicationDbContext.cs        🗄️ Contexto de Base de Datos
│   └── 📁 Repositories/                📦 Implementaciones de Repositorios
│       └── 📄 CustomerRepository.cs      🏪 Repositorio de Clientes
└── 📁 Data/
    └── 📁 Configurations/              🗄️ Configuraciones EF (futuro)
```

---

## 🗄️ **ApplicationDbContext.cs - Contexto de Base de Datos**

### **🎯 Propósito:**
Implementación concreta del contexto de Entity Framework que conecta con la base de datos.

### **📦 Características:**
- **Hereda de DbContext**: Base de Entity Framework
- **Implementa IApplicationDbContext**: Contrato de aplicación
- **DbSet&lt;Customer&gt;**: Acceso a tabla de clientes
- **SaveChangesAsync**: Persistencia de cambios

### **🔧 Configuración:**
```csharp
/// <summary>
/// Implementación del contexto de aplicación usando Entity Framework.
/// Conecta la lógica de negocio con la persistencia física.
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    /// <summary>
    /// Conjunto de entidades Customer para operaciones CRUD.
    /// Mapeado a la tabla Customers en la base de datos.
    /// </summary>
    public DbSet<Customer> Customers { get; set; }
    
    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// Operación asíncrona para mejor performance.
    /// </summary>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
```

---

## 📦 **CustomerRepository.cs - Repositorio de Clientes**

### **🎯 Propósito:**
Implementación concreta del repositorio de clientes usando Entity Framework Core.

### **🔧 Responsabilidades:**
1. **Persistir entidades Customer** en la base de datos
2. **Recuperar clientes** por su identificador único
3. **Manejar operaciones asíncronas** para mejor performance
4. **Abstraer el acceso a datos** del dominio

### **📦 Componentes del Repositorio:**

#### **🔌 Dependencias Inyectadas:**
```csharp
/// <summary>
/// Contexto de base de datos de Entity Framework.
/// Proporciona acceso a las tablas y operaciones CRUD.
/// Inyectado mediante Dependency Injection.
/// </summary>
private readonly ApplicationDbContext _context;
```

#### **🛡️ Validación de Constructor:**
```csharp
/// <summary>
/// Constructor del repositorio con inyección de dependencias.
/// </summary>
/// <param name="context">Contexto de base de datos</param>
/// <exception cref="ArgumentNullException">Lanzado si el contexto es nulo</exception>
public CustomerRepository(ApplicationDbContext context)
{
    _context = context ?? throw new ArgumentNullException(nameof(context));
}
```

#### **🔄 Métodos del Repositorio:**

##### **Add(Customer customer)**
```csharp
/// <summary>
/// Agrega un nuevo cliente a la base de datos.
/// Operación asíncrona para no bloquear el hilo principal.
/// </summary>
/// <param name="customer">Entidad Customer a persistir</param>
/// <returns>Task que representa la operación asíncrona</returns>
/// <remarks>
/// El cliente se agrega al contexto pero no se guarda inmediatamente.
/// Es necesario llamar a SaveChangesAsync() para confirmar la transacción.
/// </remarks>
public async Task Add(Customer customer) => 
    await _context.Customers.AddAsync(customer);
```

##### **GetByAsync(CustomerId id)**
```csharp
/// <summary>
/// Obtiene un cliente por su identificador único.
/// Operación asíncrona con consulta optimizada.
/// </summary>
/// <param name="id">Identificador único del cliente</param>
/// <returns>Customer encontrado o null si no existe</returns>
/// <remarks>
/// Usa SingleOrDefaultAsync para garantizar que solo se retorne un cliente
/// o null si no se encuentra. Eficiente para búsquedas por ID.
/// </remarks>
public async Task<Customer?> GetByAsync(CustomerId id) => 
    await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
```

---

## 🔄 **Flujo de Trabajo Completo**

### **📊 Secuencia de Operaciones:**
```
1. 🎯 Application Layer (CreateCustomerCommand)
   ↓
2. 📡 CreateCustomerCommandHandler
   ↓
3. 🔍 Validación de Value Objects
   ↓
4. 👤 Creación de Customer Entity
   ↓
5. 📦 CustomerRepository.Add()
   ↓
6. 🗄️ ApplicationDbContext.Customers.AddAsync()
   ↓
7. 💾 UnitOfWork.SaveChangesAsync()
   ↓
8. 🗄️ Base de Datos Física
   ↓
9. 📤 Unit Response (Success)
```

### **🎯 Ejemplo Práctico:**

#### **📥 Creación de Cliente:**
```csharp
// 1. Handler recibe comando
var command = new CreateCustomerCommand(/* datos */);

// 2. Crea Value Objects validados
var phoneNumber = PhoneNumber.Create("12345678");
var address = Address.Create("USA", "123 Main St", "", "NYC", "NY", "10001");

// 3. Crea entidad del dominio
var customer = new Customer(
    new CustomerId(Guid.NewGuid()),
    command.Name,
    command.LastName,
    command.Email,
    phoneNumber!,
    address!,
    true
);

// 4. Persiste mediante repositorio
await _customerRepository.Add(customer);

// 5. Confirma transacción
await _unitOfWork.SaveChangesAsync();
```

#### **🔍 Recuperación de Cliente:**
```csharp
// 1. Crea ID de búsqueda
var customerId = new CustomerId(Guid.Parse("guid-valor"));

// 2. Usa repositorio para buscar
var customer = await _customerRepository.GetByAsync(customerId);

// 3. Resultado: Customer encontrado o null
if (customer != null)
{
    Console.WriteLine($"Cliente: {customer.FullName}");
}
```

---

## 🎯 **Patrones y Principios Aplicados**

### **🏛️ Repository Pattern:**
- **Abstracción**: ICustomerRepository define el contrato
- **Implementación**: CustomerRepository concreta con EF
- **Desacoplamiento**: Dominio no depende de EF
- **Testabilidad**: Fácil de mock para pruebas

### **🎯 Unit of Work Pattern:**
- **Transaccionalidad**: SaveChangesAsync asegura atomicidad
- **Coordinación**: Múltiples repositorios en una transacción
- **Consistencia**: Todos o nada se guarda

### **🔧 Dependency Injection:**
- **Inversión**: Dependencias inyectadas en constructor
- **Configuración**: Registro en Startup/Program
- **Ciclo de Vida**: Gestión automática de instancias

### **🏛️ Clean Architecture:**
- **Dependencias hacia adentro**: Infrastructure depende de Domain
- **Contratos en capas internas**: Interfaces en Domain
- **Implementaciones externas**: EF en Infrastructure

---

## 🔧 **Configuración de Dependency Injection**

### **📦 Registro en Program.cs:**
```csharp
// Configurar Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar repositorios
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Configurar Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

### **🎯 Ciclos de Vida:**
- **AddDbContext**: Scoped por defecto
- **AddScoped**: Una instancia por request HTTP
- **AddSingleton**: Una instancia para toda la aplicación

---

## 🗄️ **Configuración de Entity Framework**

### **📊 Connection String:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EasyPOS;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### **🔧 Configuraciones:**
```csharp
// En ApplicationDbContext
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configuraciones de entidades
    modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    
    // Seed data inicial
    modelBuilder.Entity<Customer>().HasData(
        new Customer(/* datos iniciales */)
    );
}
```

---

## 📈 **Métricas y Performance**

### **🚀 Optimizaciones:**
- **Asíncrono**: Todas las operaciones son async
- **Consultas optimizadas**: SingleOrDefaultAsync por ID
- **Indexación**: Claves primarias automáticamente indexadas
- **Connection Pooling**: EF Core maneja conexiones eficientemente

### **📊 Monitoreo:**
- **Entity Framework Core Logging**: Configurado en desarrollo
- **Query Performance**: EF Core tracking de consultas
- **Connection Monitoring**: Logs de conexión y transacciones

---

## 🧪 **Testing Strategy**

### **📋 Unit Tests para Repositorios:**
```csharp
public class CustomerRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        _context = new ApplicationDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task Add_ShouldPersistCustomer()
    {
        // Arrange
        var customer = new Customer(/* datos válidos */);
        
        // Act
        await _repository.Add(customer);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Customers.FindAsync(customer.Id);
        Assert.NotNull(result);
        Assert.Equal(customer.Name, result.Name);
    }

    [Fact]
    public async Task GetByAsync_ShouldReturnCustomer_WhenExists()
    {
        // Arrange
        var customer = new Customer(/* datos válidos */);
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetByAsync(customer.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
    }
}
```

### **🎯 Integration Tests:**
```csharp
public class CustomerRepositoryIntegrationTests
{
    [Fact]
    public async Task AddAndGet_ShouldWorkEndToEnd()
    {
        // Arrange
        using var factory = new ApplicationFactory<Program>();
        using var scope = factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        var customer = new Customer(/* datos válidos */);
        
        // Act
        await repository.Add(customer);
        await unitOfWork.SaveChangesAsync();
        
        // Assert
        var retrieved = await repository.GetByAsync(customer.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(customer.Name, retrieved.Name);
    }
}
```

---

## 🔄 **Próximos Componentes de Infrastructure**

### **⏳ Planificados:**
1. **CustomerConfiguration.cs**: Configuración EF para Customer
2. **UnitOfWork.cs**: Implementación completa de Unit of Work
3. **Database Migrations**: Migraciones automáticas
4. **Seed Data**: Datos iniciales para desarrollo
5. **Connection Management**: Configuración de conexión

### **🎯 Mejoras Futuras:**
1. **Caching**: Redis para consultas frecuentes
2. **Auditing**: Tracking de cambios en entidades
3. **Soft Delete**: Eliminación lógica en lugar de física
4. **Batch Operations**: Operaciones en lote para mejor performance
5. **Connection Resilience**: Retry policies para fallos de conexión

---

## 📊 **Estado Actual de Infrastructure**

### **✅ Completado:**
- 🗄️ **ApplicationDbContext**: Contexto EF implementado
- 📦 **CustomerRepository**: Repositorio funcional
- 🔧 **Dependency Injection**: Configuración básica
- 📚 **Documentación**: Completa y detallada

### **⏳ En Progreso:**
- 🗄️ **Database Migrations**: Creación de esquema
- 📊 **Configurations**: Mapeos EF detallados
- 💾 **Seed Data**: Datos iniciales
- 🔍 **Query Optimization**: Índices y consultas

### **🎯 Próximos Pasos:**
1. **Crear Migrations**: `dotnet ef migrations add InitialCreate`
2. **Configurar Database**: `dotnet ef database update`
3. **Add Configurations**: Fluent API para entidades
4. **Implementar UnitOfWork**: Completa con transacciones
5. **Add Logging**: Configuración completa de EF

---

## 🌐 **Endpoints Relacionados**

### **🔧 Database Operations:**
- **Create**: `POST /api/customers` → Repository.Add()
- **Read**: `GET /api/customers/{id}` → Repository.GetByAsync()
- **Update**: `PUT /api/customers/{id}` → Repository.Update()
- **Delete**: `DELETE /api/customers/{id}` → Repository.Delete()

### **📊 Performance Endpoints:**
- **Health Check**: `/health` → Database connection
- **Metrics**: `/metrics` → Performance indicators
- **Diagnostics**: `/diagnostics` → System information

---

## 📚 **Recursos y Referencias**

### **📖 Documentación:**
- **Entity Framework Core**: https://docs.microsoft.com/en-us/ef/core/
- **Repository Pattern**: https://martinfowler.com/eaaCatalog/repository.html
- **Unit of Work**: https://martinfowler.com/eaaCatalog/unitOfWork.html
- **Dependency Injection**: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection

### **🔧 Herramientas:**
- **EF Core Tools**: `dotnet ef` para migrations
- **SQL Server Management Studio**: Administración de base de datos
- **Azure Data Studio**: Desarrollo de base de datos
- **Dapper**: Micro-ORM para consultas complejas (opcional)

---

*Última actualización: 20/02/2026*
*Versión: 1.0.0*
*Autor: EasyPOS Development Team*
