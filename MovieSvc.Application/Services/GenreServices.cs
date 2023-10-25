using Microsoft.EntityFrameworkCore;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Interface;
using MovieSvc.Domain.Entities;

namespace MovieSvc.Application.Services;

public class GenreServices: IGenreService
{
    private readonly IDbContext _db;

    public GenreServices(IDbContext db)
    {
        _db = db;
    }

    public async Task<PagedList<GenreDto>> GetAll(int pageIndex, int pageSize)
    {
        return await _db.Genre.AsQueryable()
            .Select(x =>new GenreDto()
                {
                    Name = x.Name,
                    Description = x.Description,
                    GenreId = x.Id,
                    CreationTime = x.CreationDate,
                    LastModifiedDate = x.LastModifiedDate
                }
                
            )
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize).ToPagedListAsync(pageIndex, pageSize);

    }

    private async Task<bool> NameExistBefore(string name)
    {
        return await _db.Genre.FirstOrDefaultAsync(x => x.Name == name) != null;
    }

    private async Task<bool> RunUpdate(Genre genre, GenreDto? data, bool isDeprecated)
    {
        if (data is not null)
        {
            genre.Description = data.Description;
            genre.Name = data.Name;
        }
        
        genre.IsDeprecated = isDeprecated;
        genre.UpdateLastModifiedDate(DateTime.Now);

        _db.Genre.Update(genre);

        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateUpdate(GenreDto? dto)
    {
        if (String.IsNullOrEmpty(dto?.GenreId))
        {
            bool nameExistBefore = await NameExistBefore(dto?.Name);

            if (nameExistBefore)
            {
                throw new Exception("genre name alredy exist");
            }

            Genre genre = Genre.Create(dto.Name, dto.Description);
            await _db.Genre.AddAsync(genre);

            return await _db.SaveChangesAsync() > 0;
        }

        Genre findOne = await _db.Genre.FirstOrDefaultAsync(x => x.Id == dto.GenreId);
        if (findOne is null)
        {
            throw new Exception("genre not found");
        }

        return await RunUpdate(findOne, dto, false);
        // findOne.Description = dto.Description;
        // findOne.Name = dto.Name;
        //
        // _db.Genre.Update(findOne);
        //
        // return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeprecateGenre(string genreId)
    {
        Genre findOne = await _db.Genre.FirstOrDefaultAsync(x => x.Id == genreId);
        if (findOne is null)
        {
            throw new Exception("genre not found");
        }

        return await RunUpdate(findOne, null, true);
    }

    public async Task<Genre> GetById(string genreId)
    {
        return await this._db.Genre.FirstOrDefaultAsync(x => x.Id == genreId);
    }
}