using MovieSvc.Application.Common.Model;
using MovieSvc.Domain.Entities;

namespace MovieSvc.Application.Interface;

public interface IGenreService
{
    public Task<PagedList<GenreDto>> GetAll(int pageIndex, int pageSize);
    public Task<bool> CreateUpdate(GenreDto? dto);
    public Task<bool> DeprecateGenre(string genreId);
    public Task<Genre> GetById(string genreId);
}