using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MovieSvc.Application.Handlers.Commands;
using MovieSvc.Application.Handlers.Queries;

namespace MovieSvc.Controllers;


public class GenreController: ParentController
{
    
   
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int pageSize)
    {
        var query = new GetAllGenreQuery()
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        };
       
        return  Result(await Mediator.Send(query));
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string genreId)
    {
        var query = new DeprecateGenreCommand()
        {
            GenreId = genreId
        };
       
        return  Result(await Mediator.Send(query));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreCommand command)
    {
       
        return  Result(await Mediator.Send(command));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateGenreCommand command)
    {
       
        return  Result(await Mediator.Send(command));
    }
}