using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieSvc.Application.Common.Model;

namespace MovieSvc.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParentController: Controller
{

        private IMediator _mediator;
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

    
        [NonAction]
        protected IActionResult Result<TData>(ResponseModel<TData> response)
        {
            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

      
        [NonAction]
        protected IActionResult Result(ResponseModel response)
        {
            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }
    
}