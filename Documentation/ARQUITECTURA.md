# 🏗️ Arquitectura Clean Architecture - EasyPOS

## 📋 Visión General

EasyPOS implementa **Clean Architecture** (Arquitectura Limpia) para crear un sistema mantenible, escalable y testeable. Esta arquitectura separa las responsabilidades en capas bien definidas con dependencias controladas.

## 🎯 Principios Fundamentales

### 1. **Dependencias Hacia Adentro**
- Las dependencias siempre apuntan hacia el centro
- El dominio no depende de nadie
- La infraestructura depende del dominio
- La presentación depende de la aplicación

### 2. **Separación de Responsabilidades**
- Cada capa tiene una responsabilidad específica
- No se mezcla lógica de negocio con infraestructura
- Las reglas de negocio están aisladas

### 3. **Inversión de Dependencias**
- Las capas externas dependen de abstracciones
- Las interfaces se definen en capas internas
- La implementación está en capas externas

---

## 🔄 Flujo de Datos

```
Request → Web.API → Application → Domain → Infrastructure
          ↓         ↓           ↓           ↓
       Response ← DTOs ← Use Cases ← Entities
```

### Explicación del Flujo:
1. **Web.API** recibe la petición HTTP
2. **Application** orquesta los casos de uso
3. **Domain** contiene la lógica de negocio
4. **Infrastructure** persiste los datos
5. El flujo regresa en orden inverso

---

## 📚 Capas Detalladas

## 1. 🏛️ Domain Layer (Capa de Dominio)

**Propósito:** Corazón del negocio, reglas y entidades.

**Características:**
- ✅ Sin dependencias externas
- ✅ Lógica de negocio pura
- ✅ Entidades y objetos de valor
- ✅ Interfaces de repositorios
- ✅ Reglas de negocio

**Componentes:**
```
Domain/
├── Entities/           # Entidades del negocio
│   ├── Product.cs
│   ├── Customer.cs
│   ├── Sale.cs
│   └── User.cs
├── ValueObjects/       # Objetos de valor
│   ├── Money.cs
│   ├── Email.cs
│   └── Address.cs
├── Enums/             # Enumeraciones del dominio
│   ├── SaleStatus.cs
│   └── UserRole.cs
├── Interfaces/         # Interfaces de repositorios
│   ├── IProductRepository.cs
│   ├── ICustomerRepository.cs
│   └── ISaleRepository.cs
├── Events/            # Eventos de dominio
│   ├── ProductCreated.cs
│   └── SaleCompleted.cs
└── Services/          # Servicios de dominio
    ├── PriceCalculator.cs
    └── InventoryService.cs
```

**Reglas:**
- No puede referenciar ningún otro proyecto
- Solo contiene lógica de negocio
- Las interfaces se definen aquí

---

## 2. 🎯 Application Layer (Capa de Aplicación)

**Propósito:** Orquestar casos de uso y flujo de aplicación.

**Características:**
- ✅ Casos de uso (Use Cases)
- ✅ DTOs para transferencia de datos
- ✅ Interfaces de servicios de aplicación
- ✅ Mapeos entre entidades y DTOs
- ✅ Validaciones de negocio

**Componentes:**
```
Application/
├── UseCases/          # Casos de uso
│   ├── Products/
│   │   ├── CreateProduct.cs
│   │   ├── GetProductById.cs
│   │   ├── UpdateProduct.cs
│   │   └── DeleteProduct.cs
│   ├── Customers/
│   │   ├── CreateCustomer.cs
│   │   └── GetCustomerById.cs
│   └── Sales/
│       ├── CreateSale.cs
│       └── GetSaleById.cs
├── DTOs/              # Data Transfer Objects
│   ├── ProductDto.cs
│   ├── CustomerDto.cs
│   ├── SaleDto.cs
│   └── CreateProductRequest.cs
├── Interfaces/        # Interfaces de servicios
│   ├── IProductService.cs
│   ├── ICustomerService.cs
│   └── ISaleService.cs
├── Mappings/          # Configuración de mapeos
│   ├── ProductMappingProfile.cs
│   └── CustomerMappingProfile.cs
├── Validators/        # Validaciones
│   ├── CreateProductValidator.cs
│   └── CreateCustomerValidator.cs
└── Exceptions/        # Excepciones de aplicación
    ├── ProductNotFoundException.cs
    └── InvalidProductDataException.cs
```

**Reglas:**
- Depende solo de Domain
- Contiene lógica de aplicación, no de negocio
- Define contratos para la capa de presentación

---

## 3. 🔧 Infrastructure Layer (Capa de Infraestructura)

**Propósito:** Implementar detalles técnicos y persistencia.

**Características:**
- ✅ Implementación de repositorios
- ✅ Contexto de base de datos
- ✅ Servicios externos
- ✅ Configuraciones técnicas
- ✅ Migraciones de base de datos

**Componentes:**
```
Infrastructure/
├── Data/              # Base de datos
│   ├── EasyPOSContext.cs
│   ├── Configurations/
│   │   ├── ProductConfiguration.cs
│   │   └── CustomerConfiguration.cs
│   └── Migrations/
├── Repositories/      # Implementación de repositorios
│   ├── ProductRepository.cs
│   ├── CustomerRepository.cs
│   └── SaleRepository.cs
├── Services/          # Servicios externos
│   ├── EmailService.cs
│   ├── PaymentGatewayService.cs
│   └── LoggingService.cs
├── Configurations/    # Configuraciones técnicas
│   ├── DatabaseSettings.cs
│   └── EmailSettings.cs
└── Extensions/        # Extensiones de infraestructura
    ├── ServiceCollectionExtensions.cs
    └── MiddlewareExtensions.cs
```

**Reglas:**
- Depende de Domain y Application
- Implementa interfaces definidas en capas internas
- Contiene detalles técnicos específicos

---

## 4. 🌐 Web.API Layer (Capa de Presentación)

**Propósito:** Exponer la API y manejar peticiones HTTP.

**Características:**
- ✅ Controladores API REST
- ✅ Middleware personalizado
- ✅ Configuración de HTTP
- ✅ Manejo de errores
- ✅ Documentación Swagger

**Componentes:**
```
Web.API/
├── Controllers/       # Controladores API
│   ├── ProductsController.cs
│   ├── CustomersController.cs
│   ├── SalesController.cs
│   └── AuthController.cs
├── Middleware/        # Middleware personalizado
│   ├── ExceptionHandlingMiddleware.cs
│   ├── LoggingMiddleware.cs
│   └── AuthenticationMiddleware.cs
├── Configuration/      # Configuración
│   ├── DependencyInjection.cs
│   ├── SwaggerConfiguration.cs
│   └── CorsConfiguration.cs
├── Filters/           # Filtros de acción
│   ├── ValidationFilter.cs
│   └── AuthorizationFilter.cs
├── Models/            # Modelos de API
│   ├── ApiResponse.cs
│   └── ErrorModel.cs
├── Program.cs         # Punto de entrada
├── appsettings.json   # Configuración
└── appsettings.Development.json
```

**Reglas:**
- Depende de Application e Infrastructure
- No contiene lógica de negocio
- Solo traduce peticiones HTTP a casos de uso

---

## 🔄 Dependencias Entre Proyectos

### Referencias de Proyectos
```xml
<!-- Domain.csproj -->
<!-- Sin referencias a otros proyectos -->

<!-- Application.csproj -->
<ProjectReference Include="..\Domain\Domain.csproj" />

<!-- Infrastructure.csproj -->
<ProjectReference Include="..\Domain\Domain.csproj" />
<ProjectReference Include="..\Application\Application.csproj" />

<!-- Web.API.csproj -->
<ProjectReference Include="..\Application\Application.csproj" />
<ProjectReference Include="..\Infrastructure\Infrastructure.csproj" />
```

### Comandos para Configurar Referencias
```bash
# Application depende de Domain
dotnet add Application reference Domain/Domain.csproj

# Infrastructure depende de Domain y Application
dotnet add Infrastructure reference Domain/Domain.csproj
dotnet add Infrastructure reference Application/Application.csproj

# Web.API depende de Application e Infrastructure
dotnet add Web.API reference Application/Application.csproj
dotnet add Web.API reference Infrastructure/Infrastructure.csproj
```

---

## 🎨 Patrones de Diseño Implementados

### 1. **Repository Pattern**
- Abstrae el acceso a datos
- Facilita las pruebas unitarias
- Centraliza la lógica de persistencia

### 2. **CQRS (Command Query Responsibility Segregation)**
- Separación de lecturas y escrituras
- Optimización para diferentes operaciones
- Escalabilidad independiente

### 3. **Dependency Injection**
- Inversión de control
- Configuración centralizada
- Mejor testabilidad

### 4. **Mediator Pattern**
- Desacoplamiento entre componentes
- Manejo centralizado de peticiones
- Pipeline de procesamiento

---

## 📊 Beneficios de esta Arquitectura

### ✅ **Mantenibilidad**
- Cambios en una capa no afectan a otras
- Código organizado y predecible
- Fácil de entender y modificar

### ✅ **Testabilidad**
- Cada capa puede ser probada independientemente
- Mocking de dependencias
- Pruebas unitarias y de integración claras

### ✅ **Escalabilidad**
- Capas pueden escalar independientemente
- Separación de responsabilidades
- Flexibilidad para agregar nuevas funcionalidades

### ✅ **Flexibilidad**
- Fácil cambiar tecnologías de infraestructura
- Múltiples interfaces de usuario posibles
- Adaptación a nuevos requisitos

---

## 🚀 Implementación Próxima

### Fase 1: Configuración Base
1. Configurar referencias entre proyectos
2. Configurar inyección de dependencias
3. Configurar logging y manejo de errores

### Fase 2: Dominio y Aplicación
1. Definir entidades del dominio
2. Crear interfaces de repositorios
3. Implementar casos de uso básicos

### Fase 3: Infraestructura
1. Configurar Entity Framework Core
2. Implementar repositorios
3. Crear migraciones de base de datos

### Fase 4: Presentación
1. Implementar controladores API
2. Configurar Swagger
3. Agregar validaciones y middleware

---

## 📚 Recursos Adicionales

- [Clean Architecture - Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft Architecture Guide](https://docs.microsoft.com/es-es/azure/architecture/guide/)
- [Repository Pattern MSDN](https://docs.microsoft.com/es-es/previous-versions/msp-n-p/ff649690(v=pandp.10))

---

*Última actualización: 20/02/2026*
