namespace Domain.Customers; 

/// <summary>
/// Identificador fuertemente tipado para la entidad Customer.
/// Implementado como record para garantizar inmutabilidad y comparación por valor.
/// Evita el uso de primitivos (Guid) directamente en el dominio.
/// </summary>
/// <param name="Value">Valor GUID del identificador único</param>
public record CustomerId(Guid Value);
