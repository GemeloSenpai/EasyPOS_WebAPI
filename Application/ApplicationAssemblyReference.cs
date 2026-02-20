using System.Reflection;

namespace Application;

/// <summary>
/// Referencia al ensamblado de la capa Application.
/// Utilizada para registrar servicios de MediatR y validadores.
/// </summary>
public class ApplicationAssemblyReference
{
    /// <summary>
    /// Referencia estática al ensamblado actual de Application.
    /// Usada por MediatR para descubrir handlers y eventos.
    /// </summary>
    public static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}