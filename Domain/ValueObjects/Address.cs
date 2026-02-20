namespace Domain.ValueObjects;

/// <summary>
/// Value Object para representar una dirección postal completa.
/// Implementado como partial record para flexibilidad en la estructura.
/// Los Value Objects son objetos inmutables que representan conceptos del dominio.
/// </summary>
/// <remarks>
/// Esta versión utiliza propiedades individuales en lugar de parámetros posicionales
/// para mayor flexibilidad en la creación y validación.
/// </remarks>
public partial record Address
{
    /// <summary>
    /// Constructor para inicializar una nueva instancia de Address.
    /// Establece todas las propiedades de la dirección.
    /// </summary>
    /// <param name="country">País de residencia</param>
    /// <param name="line1">Primera línea de dirección (calle y número)</param>
    /// <param name="line2">Segunda línea de dirección (apartamento, suite, etc.)</param>
    /// <param name="city">Ciudad de residencia</param>
    /// <param name="state">Estado o provincia</param>
    /// <param name="zipCode">Código postal</param>
    public Address(string country, string line1, string line2, string city, string state, string zipCode) 
    {
        Country = country;
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    /// <summary>
    /// País de residencia del cliente.
    /// Propiedad init-only para inmutabilidad.
    /// </summary>
    public string Country { get; init; }
    
    /// <summary>
    /// Primera línea de dirección (calle y número).
    /// Propiedad init-only para inmutabilidad.
    /// </summary>
    public string Line1 { get; init; }
    
    /// <summary>
    /// Segunda línea de dirección (apartamento, suite, etc.).
    /// Propiedad init-only para inmutabilidad.
    /// Puede ser vacía si no aplica.
    /// </summary>
    public string Line2 { get; init; }
    
    /// <summary>
    /// Ciudad de residencia.
    /// Propiedad init-only para inmutabilidad.
    /// </summary>
    public string City { get; init; }
    
    /// <summary>
    /// Estado o provincia.
    /// Propiedad init-only para inmutabilidad.
    /// </summary>
    public string State { get; init; }
    
    /// <summary>
    /// Código postal.
    /// Propiedad init-only para inmutabilidad.
    /// </summary>
    public string ZipCode { get; init; }

    /// <summary>
    /// Método fábrica para crear una instancia de Address con validación.
    /// Valida que todos los campos requeridos no sean nulos o vacíos.
    /// </summary>
    /// <param name="country">País (requerido)</param>
    /// <param name="line1">Primera línea (requerida)</param>
    /// <param name="line2">Segunda línea (opcional)</param>
    /// <param name="city">Ciudad (requerida)</param>
    /// <param name="state">Estado (requerido)</param>
    /// <param name="zipCode">Código postal (requerido)</param>
    /// <returns>Address válido o null si es inválido</returns>
    public static Address? Create(string country, string line1, string line2, string city, string state, string zipCode)
    {
        // Validar campos requeridos
        if (string.IsNullOrWhiteSpace(country) || 
            string.IsNullOrWhiteSpace(line1) || 
            string.IsNullOrWhiteSpace(city) || 
            string.IsNullOrWhiteSpace(state) || 
            string.IsNullOrWhiteSpace(zipCode))
        {
            return null;
        }
        
        // Crear instancia con datos limpios
        return new Address(
            country.Trim(), 
            line1.Trim(), 
            line2?.Trim() ?? "", 
            city.Trim(), 
            state.Trim(), 
            zipCode.Trim()
        );
    }

    /// <summary>
    /// Dirección completa formateada en una sola línea.
    /// Propiedad calculada para facilitar visualización.
    /// </summary>
    /// <returns>Dirección completa formateada</returns>
    public string FullAddress 
    { 
        get 
        { 
            var parts = new List<string> { Line1 };
            
            if (!string.IsNullOrWhiteSpace(Line2))
                parts.Add(Line2);
                
            parts.Add(City);
            parts.Add(State);
            parts.Add(ZipCode);
            
            if (!string.IsNullOrWhiteSpace(Country))
                parts.Add(Country);
                
            return string.Join(", ", parts);
        }
    }
}


    



