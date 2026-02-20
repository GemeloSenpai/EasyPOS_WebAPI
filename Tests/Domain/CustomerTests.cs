using Domain.Customers;
using Domain.ValueObjects;
using Xunit;

namespace EasyPOS.Tests.Domain;

/// <summary>
/// Tests unitarios para la entidad Customer.
/// Verifica el comportamiento y validaciones del dominio.
/// </summary>
public class CustomerTests
{
    [Fact]
    public void Customer_ShouldCreate_WithValidData()
    {
        // Arrange
        var id = new CustomerId(Guid.NewGuid());
        var phoneNumber = PhoneNumber.Create("123456789");
        var address = Address.Create("123 Main St", "City", "State", "12345");
        
        // Act
        var customer = new Customer(
            id, 
            "John", 
            "Doe", 
            "john@example.com", 
            phoneNumber!, 
            address!, 
            true);
        
        // Assert
        Assert.NotNull(customer);
        Assert.Equal(id, customer.Id);
        Assert.Equal("John", customer.Name);
        Assert.Equal("Doe", customer.LastName);
        Assert.Equal("john@example.com", customer.email);
        Assert.Equal("John Doe", customer.FullName);
        Assert.True(customer.Active);
    }
    
    [Fact]
    public void CustomerId_ShouldCreate_WithUniqueGuid()
    {
        // Act
        var id1 = new CustomerId(Guid.NewGuid());
        var id2 = new CustomerId(Guid.NewGuid());
        
        // Assert
        Assert.NotEqual(id1.Value, id2.Value);
        Assert.NotEqual(Guid.Empty, id1.Value);
        Assert.NotEqual(Guid.Empty, id2.Value);
    }
    
    [Fact]
    public void PhoneNumber_ShouldCreate_WithValidNumber()
    {
        // Act
        var phoneNumber = PhoneNumber.Create("12345678");
        
        // Assert
        Assert.NotNull(phoneNumber);
        Assert.Equal("12345678", phoneNumber!.Value);
    }
    
    [Fact]
    public void PhoneNumber_ShouldReturnNull_WithInvalidNumber()
    {
        // Act
        var phoneNumber = PhoneNumber.Create("123");
        
        // Assert
        Assert.Null(phoneNumber);
    }
    
    [Fact]
    public void Address_ShouldCreate_WithValidData()
    {
        // Act
        var address = Address.Create("123 Main St", "City", "State", "12345");
        
        // Assert
        Assert.NotNull(address);
        Assert.Equal("123 Main St", address!.Street);
        Assert.Equal("City", address.City);
        Assert.Equal("State", address.State);
        Assert.Equal("12345", address.ZipCode);
    }
    
    [Fact]
    public void Address_ShouldReturnNull_WithInvalidData()
    {
        // Act
        var address = Address.Create("", "City", "State", "12345");
        
        // Assert
        Assert.Null(address);
    }
}
