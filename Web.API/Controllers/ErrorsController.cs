using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.API.Controllers;

public class ErrorsController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult Error(){
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return Problem();
    }
    
}