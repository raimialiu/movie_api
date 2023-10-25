using MediatR;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;

namespace MovieSvc.Application.Handlers.Queries;

public class GetAllGenreQuery: IRequest<ResponseModel<PagedList<GenreDto>>>
{ 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
}

public class GetAllGenreQueryHandler : IRequestHandler<GetAllGenreQuery, ResponseModel<PagedList<GenreDto>>>
{
        private readonly IGenreService _genreService;
        public GetAllGenreQueryHandler(IGenreService genreService)
        {
                _genreService = genreService;
        }
        public async Task<ResponseModel<PagedList<GenreDto>>> Handle(GetAllGenreQuery request, CancellationToken cancellationToken)
        {
                var allGenre= await _genreService.GetAll(request.PageIndex, request.PageSize);
                return ResponseModel<PagedList<GenreDto>>.Success(allGenre, "Success");
        }
}