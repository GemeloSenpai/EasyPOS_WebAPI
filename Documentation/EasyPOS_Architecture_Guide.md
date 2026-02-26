# 🏗️ EasyPOS Architecture Guide - Complete Step-by-Step Analysis

## 📋 Table of Contents
1. [Project Overview](#project-overview)
2. [Clean Architecture Principles](#clean-architecture-principles)
3. [Project Structure](#project-structure)
4. [Layer-by-Layer Analysis](#layer-by-layer-analysis)
5. [Data Flow Process](#data-flow-process)
6. [Key Design Patterns](#key-design-patterns)
7. [Technology Stack](#technology-stack)
8. [Step-by-Step Operation](#step-by-step-operation)
9. [Configuration Details](#configuration-details)
10. [Best Practices Implemented](#best-practices-implemented)

---

## 🎯 Project Overview

**EasyPOS** is a Point of Sale system built with **.NET 10.0** following **Clean Architecture** principles. This architecture ensures separation of concerns, testability, and maintainability.

### Key Characteristics:
- **Framework**: .NET 10.0
- **Architecture**: Clean Architecture (Onion Architecture)
- **Pattern**: CQRS with MediatR
- **Database**: SQL Server with Entity Framework Core
- **API**: RESTful API with Swagger documentation
- **Language**: C# with modern features

---

## 🏛️ Clean Architecture Principles

### Dependency Rule
**Dependencies always point inward**:

```
┌─────────────────────────────────────────────────────────────┐
│                    Web.API (Presentation)                   │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │              Application (Use Cases)                    │ │
│  │  ┌─────────────────────────────────────────────────────┐ │ │
│  │  │                Domain (Business)                     │ │ │
│  │  │                                                     │ │ │
│  │  └─────────────────────────────────────────────────────┘ │ │
│  │                                                         │ │
│  └─────────────────────────────────────────────────────────┘ │
│                                                             │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │              Infrastructure (External)                   │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

### Layer Responsibilities:
1. **Domain**: Core business logic, entities, and rules
2. **Application**: Use cases, application services, interfaces
3. **Infrastructure**: External concerns (database, APIs, services)
4. **Web.API**: Controllers, endpoints, API documentation

---

## 📁 Project Structure

```
EasyPOS/
├── Domain/                           # Core Business Layer
│   ├── Customers/                    # Customer domain entities
│   │   ├── Customer.cs              # Main entity
│   │   ├── CustomerId.cs            # Strongly-typed ID
│   │   └── ICustomerRepository.cs   # Repository interface
│   ├── Primitives/                  # Base classes and primitives
│   │   ├── AggregateRoot.cs         # Base for entities
│   │   ├── Entity.cs               # Base entity class
│   │   └── ValueObject.cs          # Base for value objects
│   └── ValueObjects/                # Value objects
│       ├── Address.cs               # Customer address
│       └── PhoneNumber.cs           # Phone number validation
│
├── Application/                      # Use Cases Layer
│   ├── Customers/                    # Customer use cases
│   │   └── Create/                  # Create customer operations
│   │       ├── CreateCustomerCommand.cs
│   │       ├── CreateCustomerCommandHandler.cs
│   │       └── CreateCustomerCommandValidator.cs
│   ├── Data/                         # Data interfaces
│   │   └── IApplicationDbContext.cs # DB context interface
│   ├── DependencyInjection.cs       # DI configuration
│   └── ApplicationAssemblyReference.cs
│
├── Infrastructure/                   # External Layer
│   ├── Persistence/                  # Database implementation
│   │   ├── ApplicationDbContext.cs  # EF Core context
│   │   ├── Configurations/          # Entity configurations
│   │   └── Migrations/              # Database migrations
│   ├── Repositories/                 # Repository implementations
│   │   └── CustomerRepository.cs    # Customer repo implementation
│   └── Services/                    # Infrastructure services
│       ├── DependencyInjection.cs   # Infrastructure DI
│       └── UnitOfWork.cs           # Unit of work pattern
│
├── Web.API/                         # Presentation Layer
│   ├── Controllers/                  # API controllers
│   │   ├── CustomersController.cs   # Customer endpoints
│   │   ├── ApiController.cs         # Base controller
│   │   └── ErrorsController.cs     # Error handling
│   ├── Common/                      # Common API utilities
│   ├── Program.cs                   # Application entry point
│   ├── appsettings.json             # Configuration
│   └── Web.API.http                # HTTP test file
│
└── Documentation/                    # Project documentation
    ├── README.md
    ├── ARQUITECTURA.md
    ├── SWAGGER_CONFIG.md
    └── DIARIO_DESARROLLO.md
```

---

## 🔍 Layer-by-Layer Analysis

### 1. Domain Layer (Core Business)

#### Purpose:
- Contains **business logic** and **domain rules**
- Defines **entities**, **value objects**, and **aggregates**
- **No dependencies** on external frameworks
- Pure C# with business concepts

#### Key Components:

**Customer Entity** (`Domain/Customers/Customer.cs`):
```csharp
public sealed class Customer : AggregateRoot
{
    public CustomerId Id { get; private set; }
    public string Name { get; private set; }
    public string LastName { get; set; }
    public string FullName => $"{Name} {LastName}";
    public PhoneNumber PhoneNumber { get; private set; }
    public Address Address { get; private set; }
    public bool Active { get; private set; }
}
```

**Value Objects**:
- `PhoneNumber`: Validates phone format
- `Address`: Complete address information
- `CustomerId`: Strongly-typed identifier

**AggregateRoot**: Base class for managing domain events

#### Design Principles:
- **Encapsulation**: Private setters for data protection
- **Immutability**: ID cannot change after creation
- **Validation**: Value objects ensure data integrity
- **Domain Events**: AggregateRoot supports event publishing

### 2. Application Layer (Use Cases)

#### Purpose:
- Implements **business use cases**
- Defines **application interfaces**
- Orchestrates **domain objects**
- Handles **CQRS commands and queries**

#### Key Components:

**CreateCustomerCommand** (`Application/Customers/Create/CreateCustomerCommand.cs`):
```csharp
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
```

**Dependency Injection** (`Application/DependencyInjection.cs`):
```csharp
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    services.AddMediatR(config => 
    {
        config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
    });
    services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
    return services;
}
```

#### Design Patterns:
- **CQRS**: Separate commands and queries
- **MediatR**: In-process messaging
- **FluentValidation**: Input validation
- **ErrorOr**: Result pattern for error handling

### 3. Infrastructure Layer (External)

#### Purpose:
- Implements **application interfaces**
- Handles **database operations**
- Manages **external services**
- Provides **concrete implementations**

#### Key Components:

**ApplicationDbContext** (`Infrastructure/Persistence/ApplicationDbContext.cs`):
```csharp
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Customer> Customers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration for entities
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(
                id => id.Value,
                value => new CustomerId(value));
            // ... more configuration
        });
    }
}
```

**Dependency Injection** (`Infrastructure/Services/DependencyInjection.cs`):
```csharp
public static IServiceCollection AddInfrastructure(
    this IServiceCollection services, 
    IConfiguration configuration)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    
    services.AddScoped<IApplicationDbContext>(provider =>
        provider.GetRequiredService<ApplicationDbContext>());
    
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<ICustomerRepository, CustomerRepository>();
    
    return services;
}
```

#### Design Patterns:
- **Repository Pattern**: Abstract data access
- **Unit of Work**: Transaction management
- **Dependency Injection**: Service registration
- **Entity Framework Core**: ORM implementation

### 4. Web.API Layer (Presentation)

#### Purpose:
- Exposes **HTTP endpoints**
- Handles **requests and responses**
- Provides **API documentation**
- Manages **routing and middleware**

#### Key Components:

**Program.cs** (Application Entry Point):
```csharp
var builder = WebApplication.CreateBuilder(args);

// Service registration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyPOS API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

// Health checks
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.MapGet("/health/db", async (ApplicationDbContext db) =>
{
    var can = await db.Database.CanConnectAsync();
    return can ? Results.Ok(new { status = "Healthy" }) 
               : Results.Problem(statusCode: 503, detail: "Database connection failed");
});

app.Run();
```

**CustomersController** (`Web.API/Controllers/CustomersController.cs`):
```csharp
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ApiController
{
    private readonly ISender _mediator;

    public CustomersController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
    {
        var createCustomerResult = await _mediator.Send(command);
        
        return createCustomerResult.Match(
            customer => Ok(),
            errors => Problem(errors)
        );
    }
}
```

#### Features:
- **Swagger Documentation**: Auto-generated API docs
- **Health Checks**: Application and database monitoring
- **Error Handling**: Centralized error responses
- **Dependency Injection**: Service container setup

---

## 🔄 Data Flow Process

### Step-by-Step Customer Creation Flow:

#### 1. HTTP Request Arrives
```
POST /api/customers
Content-Type: application/json

{
    "name": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "+1234567890",
    "country": "USA",
    "line1": "123 Main St",
    "line2": "Apt 4B",
    "city": "New York",
    "state": "NY",
    "zipCode": "10001"
}
```

#### 2. Web.API Layer Processing
- **CustomersController.Create()** receives the request
- Validates the HTTP request structure
- Creates `CreateCustomerCommand` from request body
- Sends command to MediatR via `_mediator.Send(command)`

#### 3. Application Layer Processing
- **MediatR** finds the appropriate handler: `CreateCustomerCommandHandler`
- **FluentValidation** validates the command data
- Handler creates domain objects:
  - `PhoneNumber.Create(phoneNumber)` - validates phone format
  - `Address.Create(...)` - creates address value object
  - `new Customer(...)` - creates customer entity
- Calls repository: `_customerRepository.Add(customer)`
- Commits transaction: `_unitOfWork.SaveChangesAsync()`

#### 4. Domain Layer Processing
- **Customer** entity validates business rules
- **Value Objects** ensure data integrity
- **AggregateRoot** manages domain events (if any)

#### 5. Infrastructure Layer Processing
- **CustomerRepository.Add()** tracks the entity
- **ApplicationDbContext** manages EF Core change tracking
- **Unit of Work** commits transaction to database
- SQL Server executes the INSERT statement

#### 6. Response Flow
- Database returns success
- Unit of Work confirms commit
- Repository returns success
- Command handler returns `ErrorOr<Unit>.Success`
- MediatR returns result to controller
- Controller returns `Ok()` response
- Swagger displays successful response

---

## 🎨 Key Design Patterns

### 1. Clean Architecture (Onion Architecture)
- **Separation of Concerns**: Each layer has specific responsibilities
- **Dependency Inversion**: High-level modules don't depend on low-level modules
- **Testability**: Business logic isolated from external dependencies

### 2. CQRS (Command Query Responsibility Segregation)
```csharp
// Command (Write operation)
public record CreateCustomerCommand(...) : IRequest<ErrorOr<Unit>>;

// Query (Read operation) - Example
public record GetCustomerByIdQuery(CustomerId id) : IRequest<ErrorOr<Customer>>;
```

### 3. Repository Pattern
```csharp
// Interface in Domain layer
public interface ICustomerRepository
{
    void Add(Customer customer);
    Task<Customer?> GetByIdAsync(CustomerId id);
    Task<List<Customer>> GetAllAsync();
}

// Implementation in Infrastructure layer
public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    
    public void Add(Customer customer) => _context.Customers.Add(customer);
    // ... other methods
}
```

### 4. Unit of Work Pattern
```csharp
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
```

### 5. MediatR Pattern
```csharp
// Command
public record CreateCustomerCommand(...) : IRequest<ErrorOr<Unit>>;

// Handler
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Business logic here
        return Unit.Value;
    }
}
```

### 6. Result Pattern (ErrorOr)
```csharp
// Success case
return ErrorOr<Unit>.Success;

// Error case
return Error.Validation("Customer.Name", "Name is required");

// Usage
var result = await _mediator.Send(command);
return result.Match(
    success => Ok(),
    errors => Problem(errors)
);
```

---

## 🛠️ Technology Stack

### Core Technologies:
- **.NET 10.0**: Latest .NET framework
- **ASP.NET Core**: Web framework
- **Entity Framework Core**: ORM for database operations
- **SQL Server**: Relational database

### Key Libraries:
- **MediatR**: In-process messaging
- **FluentValidation**: Input validation
- **ErrorOr**: Result pattern implementation
- **Swashbuckle.AspNetCore**: Swagger documentation
- **Microsoft.EntityFrameworkCore**: EF Core implementation

### Development Tools:
- **Visual Studio / VS Code**: IDE
- **Git**: Version control
- **Swagger**: API documentation and testing
- **PowerShell**: Command line interface

---

## 📋 Step-by-Step Operation Guide

### 1. Project Startup Sequence

#### Step 1: Application Initialization
```csharp
// Program.cs - Entry point
var builder = WebApplication.CreateBuilder(args);
```

#### Step 2: Service Registration
```csharp
// Register API services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Register Application layer
builder.Services.AddApplication();

// Register Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);
```

#### Step 3: Application Build
```csharp
var app = builder.Build();
```

#### Step 4: Middleware Configuration
```csharp
// Development middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Production middleware
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
```

#### Step 5: Application Start
```csharp
app.Run();
```

### 2. Request Processing Flow

#### Step 1: HTTP Request Reception
- ASP.NET Core receives HTTP request
- Routing determines appropriate controller
- Model binding deserializes request body

#### Step 2: Controller Processing
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
{
    var createCustomerResult = await _mediator.Send(command);
    return createCustomerResult.Match(
        customer => Ok(),
        errors => Problem(errors)
    );
}
```

#### Step 3: MediatR Processing
- MediatR identifies command handler
- Creates handler instance via DI container
- Executes handler asynchronously

#### Step 4: Command Handler Processing
```csharp
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate input (already done by FluentValidation)
        
        // 2. Create value objects
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        var address = new Address(request.Country, request.Line1, request.Line2, 
                                request.City, request.State, request.ZipCode);
        
        // 3. Create domain entity
        var customer = new Customer(
            new CustomerId(Guid.NewGuid()),
            request.Name,
            request.LastName,
            request.Email,
            phoneNumber,
            address,
            true);
        
        // 4. Add to repository
        _customerRepository.Add(customer);
        
        // 5. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
```

#### Step 5: Database Operations
- Entity Framework tracks changes
- Generates SQL INSERT statement
- Executes against SQL Server
- Returns success/failure

#### Step 6: Response Generation
- Handler returns success result
- Controller processes result
- HTTP response sent to client

### 3. Database Operations Flow

#### Step 1: Entity Configuration
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Customer>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasConversion(
            id => id.Value,
            value => new CustomerId(value));
        // ... more configuration
    });
}
```

#### Step 2: Change Tracking
- EF Core tracks entity state
- Detects changes automatically
- Generates appropriate SQL

#### Step 3: Transaction Management
```csharp
public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    return await _context.SaveChangesAsync(cancellationToken);
}
```

#### Step 4: SQL Execution
- EF Core executes parameterized queries
- SQL Server processes the request
- Returns affected rows count

---

## ⚙️ Configuration Details

### 1. Application Configuration

#### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EasyPOS;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 2. Launch Configuration

#### launchSettings.json
```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5229",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:7062;http://localhost:5229",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 3. Project Dependencies

#### Domain Layer Dependencies:
```xml
<ItemGroup>
  <PackageReference Include="ErrorOr" Version="2.0.1" />
  <PackageReference Include="MediatR" Version="14.0.0" />
</ItemGroup>
```

#### Application Layer Dependencies:
```xml
<ItemGroup>
  <PackageReference Include="FluentValidation" Version="11.8.0" />
  <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="8.0.0" />
</ItemGroup>
```

#### Infrastructure Layer Dependencies:
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
</ItemGroup>
```

#### Web.API Layer Dependencies:
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.0" />
  <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
</ItemGroup>
```

---

## ✅ Best Practices Implemented

### 1. SOLID Principles

#### Single Responsibility Principle (SRP)
- Each class has one reason to change
- `Customer` handles business logic
- `CustomerRepository` handles data access
- `CustomersController` handles HTTP operations

#### Open/Closed Principle (OCP)
- Open for extension, closed for modification
- New use cases can be added without changing existing code
- New repositories can implement interfaces

#### Liskov Substitution Principle (LSP)
- Derived classes can replace base classes
- `ApplicationDbContext` can replace `IApplicationDbContext`

#### Interface Segregation Principle (ISP)
- Clients depend only on interfaces they use
- Separate interfaces for different concerns

#### Dependency Inversion Principle (DIP)
- High-level modules don't depend on low-level modules
- Both depend on abstractions (interfaces)

### 2. Clean Architecture Rules

#### Dependency Rule Compliance
- All dependencies point inward
- Domain layer has no external dependencies
- Infrastructure implements domain interfaces

#### Layer Isolation
- Business logic isolated from infrastructure
- UI isolated from business logic
- Database isolated from application logic

### 3. CQRS Implementation

#### Command/Query Separation
- Commands for write operations
- Queries for read operations
- Separate handlers for each

#### MediatR Benefits
- Decouples sender from receiver
- Supports request/response patterns
- Enables pipeline behaviors

### 4. Error Handling

#### Result Pattern
- `ErrorOr<T>` for success/failure handling
- Type-safe error handling
- No exceptions for business logic

#### Validation
- FluentValidation for input validation
- Automatic validation pipeline
- Clear error messages

### 5. Testing Considerations

#### Testability
- All layers can be unit tested
- Dependencies can be mocked
- Business logic isolated

#### Separation of Concerns
- Each component has single responsibility
- Easy to test individual components
- Clear interfaces for mocking

---

## 🚀 Running the Application

### 1. Prerequisites
- .NET 10.0 SDK
- SQL Server or LocalDB
- Visual Studio 2022 or VS Code

### 2. Database Setup
```bash
# Create database migration
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Web.API

# Apply migration
dotnet ef database update --project Infrastructure --startup-project Web.API
```

### 3. Run Application
```bash
# Run the API
dotnet run --project Web.API

# Application will be available at:
# - API: http://localhost:5229
# - Swagger: http://localhost:5229/swagger
```

### 4. Test with Swagger
1. Navigate to `http://localhost:5229/swagger`
2. Expand POST /api/customers endpoint
3. Click "Try it out"
4. Fill in customer data
5. Click "Execute"

---

## 📊 Summary

The EasyPOS project demonstrates a **complete Clean Architecture implementation** with:

### ✅ **Architecture Excellence**
- **Clean Architecture** principles strictly followed
- **SOLID principles** implemented throughout
- **CQRS pattern** for command/query separation
- **Domain-Driven Design** concepts applied

### ✅ **Technical Excellence**
- **Modern .NET 10.0** features utilized
- **Entity Framework Core** for data access
- **MediatR** for in-process messaging
- **FluentValidation** for input validation
- **Swagger** for API documentation

### ✅ **Maintainability**
- **Clear separation of concerns**
- **Testable code** at all layers
- **Type-safe error handling**
- **Comprehensive documentation**

### ✅ **Scalability**
- **Modular design** allows easy extension
- **Dependency injection** enables flexibility
- **Repository pattern** abstracts data access
- **Unit of Work** manages transactions

This architecture provides a solid foundation for building enterprise-grade applications that are maintainable, testable, and scalable.

---

*Document created: February 26, 2026*
*Architecture analysis: Complete EasyPOS Project*
*Version: 1.0*
