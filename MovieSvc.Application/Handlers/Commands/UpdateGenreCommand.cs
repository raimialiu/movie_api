using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Commands;

public class UpdateGenreCommand: GenreDto, IRequest<ResponseModel>
{
    
}

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, ResponseModel>
{
    private readonly IGenreService _genreService;
    public UpdateGenreCommandHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }
    public async Task<ResponseModel> Handle(UpdateGenreCommand? request, CancellationToken cancellationToken)
    {
        bool updateResponse = await _genreService.CreateUpdate(request);

        return updateResponse
            ? ResponseModel.Success("request successfull")
            : ResponseModel.Failure("failed to update");
    }
}