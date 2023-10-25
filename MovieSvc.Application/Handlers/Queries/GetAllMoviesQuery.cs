using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Queries;

public class GetAllMovieQuery: IRequest<ResponseModel<PagedList<MovieDto>>>
{ 
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetAllMovieQueryHandler : IRequestHandler<GetAllMovieQuery, ResponseModel<PagedList<MovieDto>>>
{
    private readonly IMovieService _movieService;
    public GetAllMovieQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }
    public async Task<ResponseModel<PagedList<MovieDto>>> Handle(GetAllMovieQuery request, CancellationToken cancellationToken)
    {
        var allMovies= await _movieService.GetAll(request.PageIndex, request.PageSize);
        return ResponseModel<PagedList<MovieDto>>.Success(allMovies, "Success");
    }
}