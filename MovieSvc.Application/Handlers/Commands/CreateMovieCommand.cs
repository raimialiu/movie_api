using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Commands;

public class CreateMovieCommand: MovieDto,IRequest<ResponseModel<MovieDto>>
{
    
}

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, ResponseModel<MovieDto>>
{
    private readonly IMovieService _movieService;
    public CreateMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    public async Task<ResponseModel<MovieDto>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        
        var moveiCreated= await _movieService.CreateUpdate(request);
        return ResponseModel<MovieDto>.Success(request, "Success");
    }
}