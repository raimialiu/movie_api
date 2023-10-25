using Microsoft.AspNetCore.Mvc;
using MovieSvc.Application.Handlers.Commands;
using MovieSvc.Application.Handlers.Queries;

namespace MovieSvc.Controllers;

public class MovieController: ParentController
{
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int pageSize)
    {
        var query = new GetAllMovieQuery()
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        };
       
        return  Result(await Mediator.Send(query));
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string movieId)
    {
        var query = new DeprecateMovieCommand()
        {
            MovieId = movieId
        };
       
        return  Result(await Mediator.Send(query));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovieCommand command)
    {
        return  Result(await Mediator.Send(command));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateMovieCommand command)
    {
       
        return  Result(await Mediator.Send(command));
    }
}