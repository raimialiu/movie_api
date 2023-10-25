using System.Text.Json.Serialization;
using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Handlers.Queries;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Commands;

public class DeprecateGenreCommand: IRequest<ResponseModel<bool>>
{
    [JsonPropertyName("genreId")]
    public string GenreId { get; set; }
}


public class DeprecateGenreCommandHandler : IRequestHandler<DeprecateGenreCommand, ResponseModel<bool>>
{
    private readonly IGenreService _genreService;
    public DeprecateGenreCommandHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }
    public async Task<ResponseModel<bool>> Handle(DeprecateGenreCommand request, CancellationToken cancellationToken)
    {
        var allGenre= await _genreService.DeprecateGenre(request.GenreId);
        return allGenre ? ResponseModel<bool>.Success(allGenre, "Success"):
            ResponseModel<bool>.Failure(allGenre, "failed to update");
    }
}