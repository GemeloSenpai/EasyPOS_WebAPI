# 🐍 Configuración de Swagger - EasyPOS

## 📋 Resolución del Problema

### **Problema Inicial:**
- API corriendo en `http://localhost:5229`
- Swagger UI no accesible
- Error 404 al intentar acceder a `/swagger`

### **Causa Raíz:**
- Proyecto configurado con `AddOpenApi()` (solo esquema JSON)
- Falta de paquete `Swashbuckle.AspNetCore` (interfaz visual)
- Configuración incompleta para HTTP

---

## 🛠️ Comandos Ejecutados

### **1. Instalar Paquete Swagger**
```bash
dotnet add Web.API package Swashbuckle.AspNetCore
```

**Resultado:**
- ✅ Swashbuckle.AspNetCore 10.1.4 instalado
- ✅ SwaggerGen, SwaggerUI, y dependencias agregadas
- ✅ Paquetes restaurados correctamente

### **2. Configurar Servicios en Program.cs**
**Antes:**
```csharp
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
```

**Después:**
```csharp
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyPOS API V1");
        c.RoutePrefix = "swagger";
    });
}
```

---

## 📚 Explicación de Cambios

### **Servicios Agregados:**
- **`AddEndpointsApiExplorer()`**: Descubre y describe endpoints
- **`AddSwaggerGen()`**: Genera documentación OpenAPI/Swagger

### **Middleware Configurado:**
- **`UseSwagger()`**: Expone el esquema JSON
- **`UseSwaggerUI()`**: Configura la interfaz visual

### **Configuración Específica:**
```csharp
c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyPOS API V1");
c.RoutePrefix = "swagger";
```
- Define el endpoint del esquema
- Establece la ruta base para Swagger UI

---

## 🌐 URLs de Acceso

### **Swagger UI:**
```
http://localhost:5229/swagger
```

### **Esquema JSON:**
```
http://localhost:5229/swagger/v1/swagger.json
```

### **API Endpoint:**
```
http://localhost:5229/weatherforecast
```

---

## 🔍 Verificación Funcional

### **Comandos de Verificación:**
```bash
# Verificar que el servidor está corriendo
netstat -ano | findstr 5229

# Probar endpoint directamente
curl http://localhost:5229/weatherforecast

# Verificar esquema Swagger
curl http://localhost:5229/swagger/v1/swagger.json
```

### **Resultados Esperados:**
- ✅ Servidor escuchando en puerto 5229
- ✅ Endpoint `/weatherforecast` devuelve JSON
- ✅ Swagger UI accesible en navegador
- ✅ Esquema JSON generado correctamente

---

## 📦 Paquetes Instalados

### **Swashbuckle.AspNetCore 10.1.4**
Incluye:
- **Swashbuckle.AspNetCore.Swagger**: Middleware de Swagger
- **Swashbuckle.AspNetCore.SwaggerGen**: Generador de esquemas
- **Swashbuckle.AspNetCore.SwaggerUI**: Interfaz de usuario
- **Microsoft.OpenApi**: Manipulación de especificaciones OpenAPI

### **Dependencias Automáticas:**
- Microsoft.Extensions.ApiDescription.Server
- Microsoft.OpenApi (2.4.1)

---

## ⚙️ Configuración de launchSettings.json

### **Perfil HTTP:**
```json
"http": {
  "commandName": "Project",
  "dotnetRunMessages": true,
  "launchBrowser": false,
  "applicationUrl": "http://localhost:5229",
  "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
  }
}
```

### **Perfil HTTPS:**
```json
"https": {
  "commandName": "Project",
  "dotnetRunMessages": true,
  "launchBrowser": false,
  "applicationUrl": "https://localhost:7062;http://localhost:5229",
  "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
  }
}
```

---

## 🚨 Advertencias Conocidas

### **Advertencia HTTPS:**
```
warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
      Failed to determine the https port for redirect.
```

**Explicación:**
- No afecta el funcionamiento de Swagger
- Ocurre porque el perfil HTTP no tiene configurado puerto HTTPS
- Solución: Usar perfil HTTPS o configurar redirección manual

---

## 🎯 Mejores Prácticas Implementadas

### **Configuración por Ambiente:**
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```
- Swagger solo disponible en desarrollo
- Seguridad mejorada en producción

### **Documentación Clara:**
- Nombre descriptivo de la API
- Versión especificada (V1)
- Rutas predecibles

---

## 🔄 Comandos de Desarrollo

### **Ejecutar API:**
```bash
dotnet run --project Web.API
```

### **Ejecutar con perfil específico:**
```bash
# Perfil HTTP
dotnet run --project Web.API --launch-profile "http"

# Perfil HTTPS
dotnet run --project Web.API --launch-profile "https"
```

### **Verificar estado:**
```bash
# Comprobar puertos en uso
netstat -ano | findstr 5229

# Probar endpoints
curl http://localhost:5229/weatherforecast
```

---

## 📝 Notas Importantes

### **Diferencia OpenAPI vs Swagger:**
- **OpenAPI**: Especificación del esquema
- **Swagger**: Herramientas para implementar OpenAPI
- **Swashbuckle**: Implementación de Swagger para .NET

### **Configuración Mínima Requerida:**
1. Paquete `Swashbuckle.AspNetCore`
2. `AddEndpointsApiExplorer()` y `AddSwaggerGen()`
3. `UseSwagger()` y `UseSwaggerUI()`

### **Personalización Opcional:**
- Título y descripción de la API
- Información de contacto
- Licencia y términos
- Esquemas de autenticación

---

## 🎉 Resultado Final

✅ **Swagger UI funcionando** en `http://localhost:5229/swagger`
✅ **Documentación interactiva** disponible
✅ **Endpoint de prueba** `/weatherforecast` accesible
✅ **Esquema JSON** generado correctamente
✅ **Configuración lista** para nuevos endpoints

---

*Última actualización: 20/02/2026*
