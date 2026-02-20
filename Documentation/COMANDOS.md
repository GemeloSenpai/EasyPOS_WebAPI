# 📋 Comandos Esenciales - EasyPOS

## 🚀 Comandos Iniciales (Ejecutar una sola vez)

### Configurar Solución
```bash
# Navegar al directorio del proyecto
cd C:\Proyectos_C#\EasyPOS

# Agregar todos los proyectos a la solución
dotnet sln add Domain/Domain.csproj
dotnet sln add Application/Application.csproj
dotnet sln add Infrastructure/Infrastructure.csproj
dotnet sln add Web.API/Web.API.csproj

# Verificar que los proyectos estén agregados
dotnet sln list
```

### Restaurar y Compilar
```bash
# Restaurar paquetes de toda la solución
dotnet restore

# Compilar toda la solución
dotnet build

# Verificar que no haya advertencias
dotnet build --verbosity normal
```

---

## 🛠️ Comandos de Desarrollo Diario

### Compilar y Ejecutar
```bash
# Compilar整个解决方案
dotnet build

# Ejecutar la API
dotnet run --project Web.API

# Ejecutar en modo de desarrollo
dotnet run --project Web.API --environment Development

# Compilar en modo Release
dotnet build -c Release
```

### Limpieza y Restauración
```bash
# Limpiar compilación anterior
dotnet clean

# Limpiar y recompilar
dotnet clean && dotnet build

# Restaurar paquetes específicos
dotnet restore Web.API/Web.API.csproj
```

---

## 📦 Gestión de Paquetes NuGet

### Agregar Paquetes Comunes
```bash
# Entity Framework Core (cuando se necesite)
dotnet add Infrastructure package Microsoft.EntityFrameworkCore
dotnet add Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
dotnet add Infrastructure package Microsoft.EntityFrameworkCore.Tools

# Swagger/OpenAPI
dotnet add Web.API package Swashbuckle.AspNetCore

# AutoMapper (para mapeos)
dotnet add Application package AutoMapper
dotnet add Application package AutoMapper.Extensions.Microsoft.DependencyInjection

# Validación
dotnet add Application package FluentValidation
dotnet add Application package FluentValidation.AspNetCore

# Logging
dotnet add Web.API package Serilog
dotnet add Web.API package Serilog.AspNetCore
dotnet add Web.API package Serilog.Sinks.Console
```

### Gestión de Paquetes
```bash
# Listar paquetes de un proyecto
dotnet list package Web.API

# Listar paquetes desactualizados
dotnet list package --outdated

# Eliminar paquete
dotnet remove Web.API package NombreDelPaquete
```

---

## 🧪 Comandos de Pruebas

### Crear y Ejecutar Pruebas
```bash
# Crear proyecto de pruebas (cuando se necesite)
dotnet new xunit -n EasyPOS.Tests
dotnet sln add EasyPOS.Tests/EasyPOS.Tests.csproj

# Agregar referencia al proyecto de pruebas
dotnet add EasyPOS.Tests reference Application/Application.csproj

# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas con verbosidad detallada
dotnet test --verbosity normal

# Ejecutar pruebas en modo Release
dotnet test -c Release
```

---

## 🗄️ Comandos de Base de Datos (Entity Framework Core)

### Migraciones
```bash
# Agregar herramientas de EF Core (una sola vez)
dotnet tool install --global dotnet-ef

# Crear migración inicial
dotnet ef migrations add InitialCreate --project Infrastructure

# Crear nueva migración
dotnet ef migrations add AddProductsTable --project Infrastructure

# Actualizar base de datos
dotnet ef database update --project Infrastructure

# Eliminar última migración
dotnet ef migrations remove --project Infrastructure
```

### Generación de Código
```bash
# Generar contexto desde base de datos existente
dotnet ef dbcontext scaffold "Server=.;Database=EasyPOS;Trusted_Connection=true;" \
    Microsoft.EntityFrameworkCore.SqlServer \
    --project Infrastructure \
    --output-dir Data \
    --context EasyPOSContext
```

---

## 🐛 Comandos de Depuración

### Inspección y Diagnóstico
```bash
# Ver árbol de dependencias
dotnet tree Web.API

# Analizar referencias de proyecto
dotnet list reference

# Verificar vulnerabilidades de paquetes
dotnet list package --vulnerable

# Generar reporte de compilación
dotnet build --verbosity diagnostic > build-report.txt
```

### Formato y Estilo
```bash
# Verificar formato del código
dotnet format --verify-no-changes

# Formatear todo el código
dotnet format

# Formatear proyecto específico
dotnet format Web.API/Web.API.csproj
```

---

## 📊 Comandos de Publicación

### Publicación para Producción
```bash
# Publicar para producción
dotnet publish Web.API/Web.API.csproj -c Release -o ./publish

# Publicar auto-contenida
dotnet publish Web.API/Web.API.csproj -c Release -o ./publish-self-contained \
    --self-contained true -r win-x64

# Publicar para Linux
dotnet publish Web.API/Web.API.csproj -c Release -o ./publish-linux \
    --self-contained true -r linux-x64
```

### Publicación para Docker
```bash
# Publicar para contenedor
dotnet publish Web.API/Web.API.csproj -c Release -o ./publish-docker \
    /p:GenerateRuntimeConfigurationFiles=true
```

---

## 🔧 Comandos de Configuración

### Configuración de Proyectos
```bash
# Agregar referencia entre proyectos
dotnet add Application reference Domain/Domain.csproj
dotnet add Infrastructure reference Application/Application.csproj
dotnet add Web.API reference Application/Application.csproj
dotnet add Web.API reference Infrastructure/Infrastructure.csproj

# Verificar referencias de un proyecto
dotnet list reference Application

# Crear nuevo proyecto
dotnet new classlib -n Domain
dotnet new classlib -n Application
dotnet new classlib -n Infrastructure
dotnet new webapi -n Web.API
```

---

## 🌐 Comandos de API y Red

### Ejecución y Pruebas de API
```bash
# Ejecutar API en puerto específico
dotnet run --project Web.API --urls "https://localhost:5001"

# Ejecutar con configuración específica
dotnet run --project Web.API --launch-profile "Development"

# Verificar endpoints disponibles
curl -k https://localhost:5001/swagger/v1/swagger.json

# Probar API con curl
curl -X GET "https://localhost:5001/api/products" -H "accept: application/json"
```

---

## 📝 Scripts Útiles

### Script de Compilación Completa (build.bat)
```batch
@echo off
echo Limpiando compilación anterior...
dotnet clean

echo Restaurando paquetes...
dotnet restore

echo Compilando solución...
dotnet build -c Release

echo Verificando compilación...
if %ERRORLEVEL% EQU 0 (
    echo ✅ Compilación exitosa
) else (
    echo ❌ Error en compilación
    exit /b 1
)
```

### Script de Ejecución Rápida (run.bat)
```batch
@echo off
echo Iniciando EasyPOS API...
dotnet run --project Web.API --environment Development
```

---

## ⚡ Atajos y Tips

### Atajos de Visual Studio Code
- `Ctrl+Shift+P`: Paleta de comandos
- `Ctrl+``: Abrir terminal integrada
- `F5`: Iniciar depuración
- `Ctrl+Shift+B`: Compilar proyecto

### Atajos de Línea de Comandos
- `↑/↓`: Navegar historial de comandos
- `Tab`: Autocompletar comandos y archivos
- `Ctrl+C`: Cancelar comando actual
- `cls`: Limpiar pantalla

### Variables de Entorno Útiles
```bash
# Establecer entorno de desarrollo
$env:ASPNETCORE_ENVIRONMENT="Development"

# Establecer URL personalizada
$env:ASPNETCORE_URLS="https://localhost:5001"
```

---

*Última actualización: 20/02/2026*
