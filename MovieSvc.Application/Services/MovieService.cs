using Microsoft.EntityFrameworkCore;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;
using MovieSvc.Domain.Entities;

namespace MovieSvc.Application.Services;

public class MovieService: IMovieService
{
    private readonly IDbContext _db;
    private readonly IGenreService _genreService;

    public MovieService(IDbContext db, IGenreService genreService)
    {
        _db = db;
        _genreService = genreService;
    }

    private async Task<bool> NameExistBefore(string name)
    {
        return await _db.Movies.FirstOrDefaultAsync(x => x.Name == name) != null;
    }
    
    public async Task<bool> CreateUpdate(MovieDto dto)
    {
        if (String.IsNullOrEmpty(dto?.Id))
        {
            bool nameExistBefore = await NameExistBefore(dto?.Name);

            if (nameExistBefore)
            {
                throw new Exception("genre name already exist");
            }

            Genre genre = await _genreService.GetById(dto.GenreId);
            Movie movie = Movie.Create(dto.Name, dto.Rating,dto.Photo,
                dto.Country,dto.ReleasedDate,
                 dto.TicketPrice, dto.Description, genre);
            await _db.Movies.AddAsync(movie);

            return await _db.SaveChangesAsync() > 0;
        }

        Movie findOne = await _db.Movies.FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (findOne is null)
        {
            throw new Exception("genre not found");
        }

        return await RunUpdate(findOne, dto, false);
    }
    
    private async Task<bool> RunUpdate(Movie? movie, MovieDto? data, bool isDeprecated)
    {
        if (data is not null && movie is not null)
        {
            movie.Description = data.Description;
            movie.Name = data.Name;
            movie.ReleasedDate = data.ReleasedDate;
            movie.Country = data.Country;
            movie.TicketPrice = data.TicketPrice;
        }

        if (movie is not null)
        {
            movie.IsDeprecated = isDeprecated;
            movie.UpdateLastModifiedDate(DateTime.Now);
        }

        _db.Movies.Update(movie);

        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(string movieId)
    {
        Movie? findOne = await _db.Movies.FirstOrDefaultAsync(x => x.Id == movieId);
        if (findOne is null)
        {
            throw new Exception("genre not found");
        }

        return await RunUpdate(findOne, null, true);
    }

    public async Task<PagedList<MovieDto>> GetAll(int pageIndex, int pageSize)
    {
       // Genre genre = await this._genreService.GetById();
        return await _db.Movies.AsQueryable()
            .Select(x =>new MovieDto()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Rating = x.Rating,
                    ReleasedDate = x.ReleasedDate,
                    Genre = x.Genre,
                    CreationTime = x.CreationDate,
                    LastModifiedDate = x.LastModifiedDate
                }
                
            )
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize).ToPagedListAsync(pageIndex, pageSize);
    }
}