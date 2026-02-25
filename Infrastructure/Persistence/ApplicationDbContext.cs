using Application.Data;
using Domain.Customers;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence;

/// <summary>
/// Contexto de aplicación para Entity Framework.
/// Implementa la interfaz IApplicationDbContext para abstraer el acceso a datos.
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    /// <summary>
    /// Conjunto de entidades Customer para operaciones de base de datos.
    /// </summary>
    public DbSet<Customer> Customers { get; set; }

    /// <summary>
    /// Constructor del contexto de aplicación.
    /// </summary>
    /// <param name="options">Opciones de configuración del DbContext</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Configura el modelo de entidades usando Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Builder para configurar el modelo</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurar la entidad Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new CustomerId(value));
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.email)
                .IsRequired()
                .HasMaxLength(100);
                
            // Configurar PhoneNumber como Value Object
            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasConversion(
                    phoneNumber => phoneNumber.Value,
                    value => PhoneNumber.Create(value)!);
                
            // Configurar Address como Value Object (owned entity)
            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Country)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                address.Property(a => a.Line1)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                address.Property(a => a.Line2)
                    .HasMaxLength(100);
                    
                address.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                address.Property(a => a.State)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                address.Property(a => a.ZipCode)
                    .IsRequired()
                    .HasMaxLength(20);
            });
                
            entity.Property(e => e.Active)
                .IsRequired();
        });
    }
}
