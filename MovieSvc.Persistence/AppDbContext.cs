using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using MovieSvc.Application.Interface;
using MovieSvc.Domain.Entities;
using MovieSvc.Domain.Interface;

namespace MovieSvc.Persistence;

public class AppDbContext: DbContext, IDbContext
{
    
    private readonly IDomainEventDispatcher _dispatcher;
    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
        : base(options)
    {
        _dispatcher = dispatcher;
    }
    
    public DbSet<Movie?> Movies { get; set; }
    public DbSet<Genre> Genre { get; set; }
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        SoftDeleteQueryFilters(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> enteries = ChangeTracker.Entries();

        //audit-able entity modified changes
        IEnumerable<EntityEntry> auditableEntries = enteries
            .Where(x => x.Entity.GetType().IsSubclassOf(typeof(AuditableEntity)) && x.State == EntityState.Modified);
        foreach (var entry in auditableEntries)
        {
            ((AuditableEntity)entry.Entity).UpdateLastModifiedDate(DateTime.Now);
        }

        await DispatchDomainEvents();
 
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    public override int SaveChanges()
    {
        DispatchDomainEvents().GetAwaiter().GetResult();
        return base.SaveChanges();
    }

    private static void SoftDeleteQueryFilters(ModelBuilder builder)
    {
        builder.Entity<Movie>().HasQueryFilter(e => e.IsDeprecated == false);
        builder.Entity<Genre>().HasQueryFilter(e => e.IsDeprecated == false);
    }
    
    private async Task DispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<IEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            while (entity.DomainEvents.TryTake(out var domainEvent))
            {
                await _dispatcher.Dispatch(domainEvent);
            }
        }
    }
}