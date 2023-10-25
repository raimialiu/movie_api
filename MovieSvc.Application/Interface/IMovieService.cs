using MovieSvc.Application.Common.Model;

namespace MovieSvc.Application.Interface;

public interface IMovieService
{
    Task<bool> CreateUpdate(MovieDto dto);
    Task<bool> Delete(string movieId);
    Task<PagedList<MovieDto>> GetAll(int pageIndex, int pageSize);
}