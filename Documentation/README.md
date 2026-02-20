# EasyPOS - Documentación del Proyecto

## 📋 Índice de Documentación

- [Arquitectura Limpia (Clean Architecture)](#arquitectura-limpia-clean-architecture)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Configuración del Entorno](#configuración-del-entorno)
- [Comandos Útiles](#comandos-útiles)
- [API REST Endpoints](#api-rest-endpoints)
- [Configuración de Swagger](#configuración-de-swagger)
- [Endpoints Actuales](#endpoints-actuales)
- [Primitivas de Dominio](#primitivas-de-dominio)
- [Diario de Desarrollo](#diario-de-desarrollo)

---

## 🏗️ Arquitectura Limpia (Clean Architecture)

EasyPOS sigue los principios de Clean Architecture para mantener un código escalable, mantenible y testeable.

### Capas de la Arquitectura

1. **Domain Layer** (Capa de Dominio)
   - Entidades del negocio
   - Interfaces de repositorios
   - Lógica de negocio central
   - Sin dependencias externas

2. **Application Layer** (Capa de Aplicación)
   - Casos de uso (Use Cases)
   - Interfaces de servicios
   - DTOs y mapeos
   - Orquestación de flujos de negocio

3. **Infrastructure Layer** (Capa de Infraestructura)
   - Implementación de repositorios
   - Conexión a base de datos
   - Servicios externos
   - Configuraciones técnicas

4. **Presentation Layer** (Capa de Presentación)
   - Controladores API REST
   - Middleware
   - Configuración de HTTP
   - Manejo de errores

---

## 📁 Estructura del Proyecto

```
EasyPOS/
├── Domain/                    # Capa de Dominio
│   ├── Domain.csproj
│   ├── Entities/             # Entidades del negocio
│   ├── Interfaces/           # Interfaces de repositorios
│   └── ValueObjects/         # Objetos de valor
├── Application/              # Capa de Aplicación
│   ├── Application.csproj
│   ├── UseCases/            # Casos de uso
│   ├── Interfaces/          # Interfaces de servicios
│   ├── DTOs/                # Data Transfer Objects
│   └── Mappings/            # Mapeos entre entidades
├── Infrastructure/           # Capa de Infraestructura
│   ├── Infrastructure.csproj
│   ├── Data/                # Contexto de base de datos
│   ├── Repositories/        # Implementación de repositorios
│   └── Services/            # Servicios externos
├── Web.API/                 # Capa de Presentación
│   ├── Web.API.csproj
│   ├── Controllers/         # Controladores API
│   ├── Middleware/          # Middleware personalizado
│   ├── Configuration/       # Configuración
│   └── Program.cs          # Punto de entrada
├── Documentation/           # Documentación del proyecto
└── EasyPOS.slnx            # Archivo de solución
```

---

## ⚙️ Configuración del Entorno

### Prerrequisitos
- .NET 10.0 SDK
- Visual Studio 2022 o VS Code
- Git

### Configuración Inicial

#### 1. Restaurar Proyectos en la Solución
```bash
# Agregar proyectos a la solución (ejecutar una sola vez)
dotnet sln add Domain/Domain.csproj
dotnet sln add Application/Application.csproj
dotnet sln add Infrastructure/Infrastructure.csproj
dotnet sln add Web.API/Web.API.csproj

# Verificar proyectos en la solución
dotnet sln list
```

#### 2. Restaurar Paquetes NuGet
```bash
# Restaurar todos los paquetes de la solución
dotnet restore

# O restaurar por proyecto específico
dotnet restore Web.API/Web.API.csproj
```

#### 3. Compilar el Proyecto
```bash
# Compilar toda la solución
dotnet build

# Compilar en modo Release
dotnet build -c Release

# Compilar proyecto específico
dotnet build Web.API/Web.API.csproj
```

#### 4. Ejecutar la Aplicación
```bash
# Ejecutar el proyecto Web.API
dotnet run --project Web.API

# Ejecutar en modo específico
dotnet run --project Web.API --environment Development
```

---

## 🛠️ Comandos Útiles

### Desarrollo
```bash
# Limpiar compilación anterior
dotnet clean

# Compilar y ejecutar pruebas
dotnet test

# Verificar formato del código
dotnet format --verify-no-changes

# Formatear código automáticamente
dotnet format
```

### Gestión de Paquetes
```bash
# Agregar paquete a proyecto específico
dotnet add Web.API package Microsoft.EntityFrameworkCore

# Agregar paquete a todos los proyectos
dotnet sln add package Microsoft.EntityFrameworkCore

# Listar paquetes de un proyecto
dotnet list package Web.API
```

### Base de Datos (cuando se implemente)
```bash
# Crear migración (ejemplo futuro)
dotnet ef migrations add InitialCreate --project Infrastructure

# Actualizar base de datos
dotnet ef database update --project Infrastructure
```

---

## 🐍 Configuración de Swagger

### **Problema Resuelto:**
- ✅ Swagger UI configurado correctamente
- ✅ Interfaz accesible en `http://localhost:5229/swagger`
- ✅ Documentación interactiva funcionando

### **Comandos Clave:**
```bash
# Instalar Swagger
dotnet add Web.API package Swashbuckle.AspNetCore

# Configurar servicios en Program.cs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

# Configurar middleware
app.UseSwagger();
app.UseSwaggerUI();
```

**Documentación completa:** [SWAGGER_CONFIG.md](SWAGGER_CONFIG.md)

---

## 🌐 Endpoints Actuales

### **Endpoint Disponible:**
```http
GET /weatherforecast
```
- **URL:** `http://localhost:5229/weatherforecast`
- **Response:** Array JSON con pronóstico del tiempo
- **Documentación:** Disponible en Swagger UI

### **Próximos Endpoints Planificados:**
- Productos CRUD
- Clientes CRUD  
- Ventas CRUD
- Autenticación

**Documentación completa:** [ENDPOINTS_ACTUALES.md](ENDPOINTS_ACTUALES.md)

---

## 🏗️ Primitivas de Dominio

### **Patrones DDD Implementados:**
- ✅ **DomainEvent**: Base para eventos de dominio con MediatR
- ✅ **AggregateRoot**: Raíz de agregado con manejo de eventos
- ✅ **IUnitOfWork**: Unidad de trabajo para transacciones

### **Características Principales:**
- **Eventos asíncronos** con MediatR
- **Transacciones atómicas** con Unit of Work
- **Documentación XML** en código fuente
- **Clean Architecture** sin dependencias externas

### **Componentes Técnicos:**
```csharp
// Base para eventos
public record DomainEvent(Guid Id): INotification;

// Raíz de agregado
public abstract class AggregateRoot
{
    protected void Raise(DomainEvent domainEvent) { }
    public ICollection<DomainEvent> GetDomainEvents() { }
}

// Unidad de trabajo
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

**Documentación completa:** [PRIMITIVAS_DOMINIO.md](PRIMITIVAS_DOMINIO.md)

---

## 📖 Diario de Desarrollo

### **Registro del Proyecto:**
- **Día 1**: Configuración inicial y Swagger
- **Clean Architecture**: Estructura y dependencias
- **DDD Patterns**: Primitivas de dominio implementadas
- **Git/GitHub**: Control de versiones configurado

### **Lecciones Aprendidas:**
- Diferencia entre OpenAPI y Swagger
- Patrones de Domain-Driven Design
- Mejores prácticas de documentación
- Flujo de trabajo con Git

**Documentación completa:** [DIARIO_DESARROLLO.md](DIARIO_DESARROLLO.md)

---

## 📝 Buenas Prácticas Implementadas

### Clean Code
- ✅ Nombres descriptivos de variables y métodos
- ✅ Funciones pequeñas y con una responsabilidad
- ✅ Comentarios explicativos cuando sea necesario
- ✅ Formato consistente del código

### Clean Architecture
- ✅ Dependencias hacia adentro
- ✅ Separación de responsabilidades
- ✅ Inyección de dependencias
- ✅ Principio de inversión de dependencias

### API REST
- ✅ Verbos HTTP adecuados
- ✅ Códigos de estado estándar
- ✅ Nombres de recursos en plural
- ✅ Versionamiento de API (planificado)

---

## 🚀 Próximos Pasos

1. **Configurar dependencias entre proyectos**
2. **Implementar entidades de dominio**
3. **Crear casos de uso básicos**
4. **Configurar base de datos**
5. **Implementar endpoints API**
6. **Agregar autenticación y autorización**
7. **Implementar logging y manejo de errores**
8. **Agregar pruebas unitarias y de integración**

---

## 📚 Recursos de Aprendizaje

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft .NET Documentation](https://docs.microsoft.com/es-es/dotnet/)
- [ASP.NET Core Web API Documentation](https://docs.microsoft.com/es-es/aspnet/core/web-api/)
- [REST API Design Guidelines](https://restfulapi.net/)

---

*Última actualización: 20/02/2026*
