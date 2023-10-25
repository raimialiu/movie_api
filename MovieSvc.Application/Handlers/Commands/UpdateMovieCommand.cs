using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Commands;


public class UpdateMovieCommand: MovieDto, IRequest<ResponseModel>
{
    
}

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, ResponseModel>
{
    private readonly IMovieService _movieService;
    public UpdateMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }
    public async Task<ResponseModel> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        bool updateResponse = await _movieService.CreateUpdate(request);

        return updateResponse
            ? ResponseModel.Success("request successfull")
            : ResponseModel.Failure("failed to update");
    }
}