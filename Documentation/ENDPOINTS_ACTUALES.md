# 🌐 Endpoints Actuales - EasyPOS API

## 📋 Estado Actual de la API

### **Configuración Base:**
- **Framework**: ASP.NET Core 10.0
- **URL Base**: `http://localhost:5229`
- **Documentación**: Swagger UI disponible
- **Ambiente**: Development

---

## 🚀 Endpoints Disponibles

### **1. Weather Forecast**
```http
GET /weatherforecast
```

**Descripción:** Endpoint de ejemplo para pronóstico del tiempo

**Respuesta Exitosa (200 OK):**
```json
[
  {
    "date": "2026-02-21",
    "temperatureC": 22,
    "summary": "Warm",
    "temperatureF": 71
  },
  {
    "date": "2026-02-22", 
    "temperatureC": 34,
    "summary": "Balmy",
    "temperatureF": 93
  },
  {
    "date": "2026-02-23",
    "temperatureC": 39,
    "summary": "Sweltering", 
    "temperatureF": 102
  },
  {
    "date": "2026-02-24",
    "temperatureC": 28,
    "summary": "Bracing",
    "temperatureF": 82
  },
  {
    "date": "2026-02-25",
    "temperatureC": 4,
    "summary": "Scorching",
    "temperatureF": 39
  }
]
```

**Estructura del Response:**
- **date**: Fecha del pronóstico (YYYY-MM-DD)
- **temperatureC**: Temperatura en Celsius
- **summary**: Descripción del clima
- **temperatureF**: Temperatura en Fahrenheit (calculada)

---

## 📚 Documentación API

### **Swagger UI:**
```
http://localhost:5229/swagger
```

**Características:**
- ✅ Interfaz interactiva
- ✅ Documentación auto-generada
- ✅ Probador de endpoints integrado
- ✅ Esquemas JSON detallados

### **OpenAPI Schema:**
```
http://localhost:5229/swagger/v1/swagger.json
```

---

## 🛠️ Comandos de Prueba

### **Prueba con curl:**
```bash
# Probar endpoint
curl http://localhost:5229/weatherforecast

# Con formato JSON legible
curl http://localhost:5229/weatherforecast | jq

# Con headers detallados
curl -v http://localhost:5229/weatherforecast
```

### **Prueba con PowerShell:**
```powershell
# Invoke-RestMethod
Invoke-RestMethod -Uri "http://localhost:5229/weatherforecast" -Method GET

# Con formato
(Invoke-RestMethod -Uri "http://localhost:5229/weatherforecast" -Method GET) | ConvertTo-Json -Depth 10
```

### **Prueba en Navegador:**
- Acceder directamente a `http://localhost:5229/weatherforecast`
- Usar Swagger UI en `http://localhost:5229/swagger`

---

## 📊 Esquema del Modelo

### **WeatherForecast Model:**
```csharp
public record WeatherForecast(
    DateOnly Date,
    int TemperatureC, 
    string? Summary
)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
```

**Propiedades:**
- **Date** (DateOnly): Fecha del pronóstico
- **TemperatureC** (int): Temperatura en grados Celsius
- **Summary** (string?): Descripción textual del clima
- **TemperatureF** (int, calculada): Temperatura en Fahrenheit

---

## 🔄 Flujo de Solicitud Actual

```
Cliente HTTP → Web.API → Minimal API → Response
     ↓              ↓           ↓          ↓
  Browser    Program.cs    MapGet()    JSON Array
```

**Explicación:**
1. Cliente realiza petición GET
2. ASP.NET Core enruta a `/weatherforecast`
3. Minimal API genera datos aleatorios
4. Response serializada a JSON

---

## 🎯 Próximos Endpoints Planificados

### **Módulo de Productos:**
```http
GET    /api/products           # Listar productos
GET    /api/products/{id}      # Obtener producto por ID
POST   /api/products           # Crear producto
PUT    /api/products/{id}      # Actualizar producto
DELETE /api/products/{id}      # Eliminar producto
```

### **Módulo de Clientes:**
```http
GET    /api/customers          # Listar clientes
GET    /api/customers/{id}     # Obtener cliente por ID
POST   /api/customers          # Crear cliente
PUT    /api/customers/{id}     # Actualizar cliente
DELETE /api/customers/{id}     # Eliminar cliente
```

### **Módulo de Ventas:**
```http
GET    /api/sales              # Listar ventas
GET    /api/sales/{id}         # Obtener venta por ID
POST   /api/sales              # Crear venta
PUT    /api/sales/{id}         # Actualizar venta
```

---

## 📈 Métricas Actuales

### **Rendimiento:**
- ✅ **Tiempo de respuesta**: < 50ms
- ✅ **Estado**: 200 OK
- ✅ **Content-Type**: application/json

### **Logs de Startup:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5229
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Proyectos_C#\EasyPOS\Web.API
```

---

## 🔍 Herramientas de Depuración

### **Verificación de Estado:**
```bash
# Comprobar que el servidor está corriendo
netstat -ano | findstr 5229

# Verificar respuesta HTTP
curl -I http://localhost:5229/weatherforecast

# Probar conectividad
Test-NetConnection -ComputerName localhost -Port 5229
```

### **Headers de Respuesta:**
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 20 Feb 2026 16:14:00 GMT
Server: Kestrel
Transfer-Encoding: chunked
```

---

## 🚀 Comandos de Ejecución

### **Iniciar API:**
```bash
dotnet run --project Web.API
```

### **Ejecutar en modo específico:**
```bash
# Desarrollo (default)
dotnet run --project Web.API --environment Development

# Producción
dotnet run --project Web.API --environment Production
```

### **Compilar y Ejecutar:**
```bash
dotnet build && dotnet run --project Web.API
```

---

## 📝 Notas de Desarrollo

### **Características Actuales:**
- ✅ **Minimal API**: Configuración básica funcional
- ✅ **Swagger Documentation**: Completa y accesible
- ✅ **Clean Architecture**: Estructura preparada
- ✅ **Development Environment**: Configurado para desarrollo

### **Limitaciones Actuales:**
- ⚠️ **Solo un endpoint**: Solo `/weatherforecast` disponible
- ⚠️ **Datos estáticos**: Sin persistencia de datos
- ⚠️ **Sin autenticación**: No hay seguridad implementada
- ⚠️ **Sin validación**: No hay validación de entrada

---

## 🎯 Siguientes Pasos

1. **Crear entidades de dominio** en capa Domain
2. **Implementar casos de uso** en capa Application  
3. **Configurar base de datos** en capa Infrastructure
4. **Crear controladores** con endpoints RESTful
5. **Agregar validaciones** y manejo de errores
6. **Implementar autenticación** y autorización

---

*Última actualización: 20/02/2026*
