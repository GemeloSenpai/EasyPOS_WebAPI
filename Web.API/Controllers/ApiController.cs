using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Web.API.Common.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.API.Controllers;

/// <summary>
/// Controlador base para todos los controladores de la API.
/// Proporciona métodos centralizados para manejo de errores con Problem Details.
/// Implementa el patrón Template Method para consistencia en respuestas.
/// </summary>
//
[ApiController]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Maneja una lista de errores y retorna la respuesta Problem Details apropiada.
    /// Centraliza el manejo de errores para mantener consistencia en toda la API.
    /// </summary>
    /// <param name="errors">Lista de errores a procesar</param>
    /// <returns>IActionResult con Problem Details configurado</returns>
    protected IActionResult Problem(List<Error> errors)
    {
        // Si no hay errores, retornar respuesta por defecto
        if(errors.Count is 0)
        {
            return Problem();
        }
        
        // Si todos son errores de validación, usar ValidationProblem
        if(errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }
        
        // Almacenar errores en HttpContext para procesamiento en la fábrica
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        // Retornar el primer error como problema principal
        return Problem(errors[0]);
    }
    
    /// <summary>
    /// Convierte un Error individual en una respuesta Problem Details.
    /// Mapea tipos de error a códigos de estado HTTP apropiados.
    /// </summary>
    /// <param name="error">Error a procesar</param>
    /// <returns>IActionResult con Problem Details configurado</returns>
    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,     // Conflicto de recursos
            ErrorType.NotFound => StatusCodes.Status404NotFound,       // Recurso no encontrado
            ErrorType.Validation => StatusCodes.Status400BadRequest,   // Error de validación
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized, // No autorizado
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,     // Prohibido
            _ => StatusCodes.Status500InternalServerError,           // Error interno por defecto
        };

        return Problem(
            statusCode: statusCode, 
            title: error.Description,
            detail: error.Description
        );
    }

    /// <summary>
    /// Crea una respuesta de validación con múltiples errores.
    /// Convierte la lista de errores en un ModelStateDictionary.
    /// </summary>
    /// <param name="errors">Lista de errores de validación</param>
    /// <returns>ValidationProblemResult con errores detallados</returns>
    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        
        // Agregar cada error al ModelState con su código como clave
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }
        
        return ValidationProblem(modelStateDictionary);
    }
}