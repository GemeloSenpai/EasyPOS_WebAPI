using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

/// <summary>
/// Factory para crear ApplicationDbContext en tiempo de diseño.
/// Utilizado por las herramientas de Entity Framework Core en tiempo de diseño.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Crea una instancia de ApplicationDbContext para uso en tiempo de diseño.
    /// </summary>
    /// <param name="args">Argumentos de línea de comandos</param>
    /// <returns>Instancia de ApplicationDbContext configurada</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Usar SQL Server GemeloSensei para desarrollo
        optionsBuilder.UseSqlServer("Server=GemeloSensei;Database=EASYPOS;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true");
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
