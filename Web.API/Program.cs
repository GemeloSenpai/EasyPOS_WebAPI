using Application;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de la capa de aplicación
builder.Services.AddApplication();

// Configuración de la capa de infraestructura
builder.Services.AddInfrastructure(builder.Configuration);

// Configuración de controladores
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configuración del middleware pipeline
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyPOS API V1");
        c.RoutePrefix = "swagger";
    });
}

// Redirección automática a HTTPS
app.UseHttpsRedirection();

// Habilitar routing y endpoints
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Endpoint de ejemplo para WeatherForecast
// Este endpoint es solo para demostración y pruebas
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    // Generar datos de pronóstico del tiempo para demostración
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Health checks
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.MapGet("/health/db", async (Infrastructure.Persistence.ApplicationDbContext db) =>
{
    try
    {
        var can = await db.Database.CanConnectAsync();
        return can ? Results.Ok(new { status = "Healthy" }) : Results.Problem(statusCode: 503, detail: "Database connection failed");
    }
    catch (Exception ex)
    {
        return Results.Problem(statusCode: 500, detail: ex.Message);
    }
});

// Iniciar la aplicación
app.Run();

// Record para el endpoint WeatherForecast
// Define la estructura de datos para el pronóstico del tiempo
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    // Propiedad calculada para convertir Celsius a Fahrenheit
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
