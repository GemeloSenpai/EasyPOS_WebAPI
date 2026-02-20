using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

/// <summary>
/// Value Object para representar un número de teléfono válido.
/// Los Value Objects son objetos inmutables que representan
/// conceptos del dominio y se comparan por valor.
/// </summary>
public partial record PhoneNumber
{
    /// <summary>
    /// Longitud por defecto para números de teléfono (9 dígitos).
    /// </summary>
    private const int DefaultLength = 9;

    /// <summary>
    /// Patrón de expresión regular para validar formato de teléfono.
    /// Acepta números con guiones opcionales.
    /// </summary>
    private const string Pattern = @"^(?:-*\d-*){8}$";

    /// <summary>
    /// Constructor privado para asegurar creación controlada.
    /// Solo se puede crear a través del método Create().
    /// </summary>
    /// <param name="value">Valor del número de teléfono</param>
    private PhoneNumber(string value) => Value = value;

    /// <summary>
    /// Método fábrica para crear una instancia de PhoneNumber.
    /// Valida el formato y retorna null si es inválido.
    /// </summary>
    /// <param name="value">Cadena de texto a convertir</param>
    /// <returns>PhoneNumber válido o null si es inválido</returns>
    public static PhoneNumber? Create(string value)
    {
        // Validar que no sea nulo o vacío o cumpla el patron
        if (string.IsNullOrEmpty(value) || !PhoneNumberRegex().IsMatch(value) || value.Length != DefaultLength)
        {
            return null;
        }
           
        return new PhoneNumber(value);
    }

    /// <summary>
    /// Valor del número de teléfono una vez validado.
    /// Init-only para asegurar inmutabilidad.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Método parcial para generar la expresión regular.
    /// Usando partial para optimización del compilador.
    /// </summary>
    /// <returns>Regex compilada para validación</returns>
    private static partial Regex PhoneNumberRegex();
}

