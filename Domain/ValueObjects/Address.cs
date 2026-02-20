namespace Domain.ValueObjects;

/// <summary>
/// Value Object para representar una dirección postal completa.
/// Los Value Objects son objetos inmutables que representan
/// conceptos del dominio y se comparan por valor.
/// </summary>
/// <param name="Street">Calle y número de la dirección</param>
/// <param name="City">Ciudad de la dirección</param>
/// <param name="State">Estado o provincia</param>
/// <param name="ZipCode">Código postal</param>
/// <param name="Country">País (opcional, por defecto vacío)</param>
public record Address(
    string Street,
    string City, 
    string State,
    string ZipCode,
    string Country = ""
)
{
    /// <summary>
    /// Método fábrica para crear una instancia de Address.
    /// Valida que todos los campos requeridos no sean nulos o vacíos.
    /// </summary>
    /// <param name="street">Calle y número</param>
    /// <param name="city">Ciudad</param>
    /// <param name="state">Estado</param>
    /// <param name="zipCode">Código postal</param>
    /// <param name="country">País (opcional)</param>
    /// <returns>Address válido o null si es inválido</returns>
    public static Address? Create(string street, string city, string state, string zipCode, string country = "")
    {
        if (string.IsNullOrWhiteSpace(street) || 
            string.IsNullOrWhiteSpace(city) || 
            string.IsNullOrWhiteSpace(state) || 
            string.IsNullOrWhiteSpace(zipCode))
        {
            return null;
        }
        
        return new Address(street.Trim(), city.Trim(), state.Trim(), zipCode.Trim(), country?.Trim() ?? "");
    }
    
    /// <summary>
    /// Dirección completa formateada en una sola línea.
    /// Propiedad calculada para facilitar visualización.
    /// </summary>
    public string FullAddress => string.IsNullOrEmpty(Country) 
        ? $"{Street}, {City}, {State} {ZipCode}"
        : $"{Street}, {City}, {State} {ZipCode}, {Country}";
}
