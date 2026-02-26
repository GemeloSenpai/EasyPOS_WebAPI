using Application.Customers.Create;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ApiController
{
   private readonly ISender _mediator;

   public CustomersController(ISender mediator)
   {
      _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
   }

   [HttpPost]
   public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command){
      var createCustomerResult = await _mediator.Send(command);
      
      return createCustomerResult.Match(
         customer => Ok(),
         errors => Problem(errors)
      );
   }
}
