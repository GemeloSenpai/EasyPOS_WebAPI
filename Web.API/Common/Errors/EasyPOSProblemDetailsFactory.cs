using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.API.Common.Http;
using System.Diagnostics;
using ErrorOr;

namespace Web.API.Common.Errors;

/// <summary>
/// Fábrica personalizada para crear Problem Details según RFC 7807.
/// Proporciona respuestas de error consistentes y enriquecidas con contexto.
/// </summary>
public class EasyPOSProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;

    /// <summary>
    /// Constructor de la fábrica de Problem Details.
    /// </summary>
    /// <param name="options">Opciones de comportamiento de API</param>
    /// <exception cref="ArgumentNullException">Lanzado si options es nulo</exception>
    public EasyPOSProblemDetailsFactory(ApiBehaviorOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }
    
    /// <summary>
    /// Crea un ProblemDetails para errores generales.
    /// Aplica valores por defecto y enriquece con información de contexto.
    /// </summary>
    /// <param name="httpContext">Contexto HTTP actual</param>
    /// <param name="statusCode">Código de estado HTTP</param>
    /// <param name="title">Título del error</param>
    /// <param name="type">URI que identifica el tipo de problema</param>
    /// <param name="detail">Descripción detallada del error</param>
    /// <param name="instance">URI que identifica la ocurrencia específica</param>
    /// <returns>ProblemDetails configurado</returns>
    public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, 
        string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        statusCode ??= 500;
        
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };
        
        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
        
        return problemDetails;
    }

    /// <summary>
    /// Crea un ValidationProblemDetails para errores de validación.
    /// Incluye los errores del ModelState en la respuesta.
    /// </summary>
    /// <param name="httpContext">Contexto HTTP actual</param>
    /// <param name="modelStateDictionary">ModelState con errores de validación</param>
    /// <param name="statusCode">Código de estado HTTP</param>
    /// <param name="title">Título del error</param>
    /// <param name="type">URI que identifica el tipo de problema</param>
    /// <param name="detail">Descripción detallada del error</param>
    /// <param name="instance">URI que identifica la ocurrencia específica</param>
    /// <returns>ValidationProblemDetails configurado</returns>
    /// <exception cref="ArgumentNullException">Lanzado si modelStateDictionary es nulo</exception>
    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, 
        ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, 
        string? type = null, string? detail = null, string? instance = null)
    {
        if(modelStateDictionary == null)
        {
            throw new ArgumentNullException(nameof(modelStateDictionary));
        }
        
        statusCode ??= 400;
        
        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        if(title != null)
        {
            problemDetails.Title = title;
        }
        
        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
        
        return problemDetails;
    }

    /// <summary>
    /// Aplica valores por defecto a los Problem Details.
    /// Incluye trace ID para debugging y códigos de error personalizados.
    /// </summary>
    /// <param name="httpContext">Contexto HTTP actual</param>
    /// <param name="problemDetails">Problem Details a configurar</param>
    /// <param name="statusCode">Código de estado HTTP</param>
    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        // Aplicar mapeo de errores de cliente por defecto
        if(_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        // Agregar trace ID para debugging
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        // Agregar códigos de error personalizados desde HttpContext
        var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;
        if (errors != null && errors.Count > 0)
        {
            problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
        }
    }
}