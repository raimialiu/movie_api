using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieSvc.Domain.Entities;

namespace MovieSvc.Application.Interface;

public interface IDbContext: IDisposable
{
    DbSet<Movie?> Movies { get; set; }
    DbSet<Genre> Genre { get; set; }
    
    EntityEntry Entry([NotNull] object entity);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}