using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Commands;

public class CreateGenreCommand: GenreDto,IRequest<ResponseModel<GenreDto>>
{
    
}

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, ResponseModel<GenreDto>>
{
    private readonly IGenreService _genreService;
    public CreateGenreCommandHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }
    
    public async Task<ResponseModel<GenreDto>> Handle(CreateGenreCommand? request, CancellationToken cancellationToken)
    {
        
        var allGenre= await _genreService.CreateUpdate(request);
        return ResponseModel<GenreDto>.Success(request, "Success");
    }
}