using System.Text.Json.Serialization;
using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Commands;


public class DeprecateMovieCommand: IRequest<ResponseModel<bool>>
{
  
    public string MovieId { get; set; }
}


public class DeprecateMovieCommandHandler : IRequestHandler<DeprecateMovieCommand, ResponseModel<bool>>
{
    private readonly IMovieService _movieService;
    public DeprecateMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }
    public async Task<ResponseModel<bool>> Handle(DeprecateMovieCommand request, CancellationToken cancellationToken)
    {
        var allGenre= await _movieService.Delete(request.MovieId);
        return allGenre ? ResponseModel<bool>.Success(allGenre, "Success"):
            ResponseModel<bool>.Failure(allGenre, "failed to update");
    }
}