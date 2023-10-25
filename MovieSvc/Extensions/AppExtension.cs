using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MovieSvc.Application.Common.Behaviour;
using MovieSvc.Application.Common.Model;
using MovieSvc.Application.Helpers;
using MovieSvc.Application.Interface;
using MovieSvc.Application.Services;
using Polly;
using Polly.Extensions.Http;

namespace MovieSvc.Extensions;

public static class AppExtension
{
    
    public static string ModelValidationErrorMessages(this ModelStateDictionary modelStateDictionary)
    {
        List<string> errors = new List<string>();
        string messages = string.Join("; ", modelStateDictionary.Values
            .SelectMany(x => x.Errors)
            .Select(x => x.ErrorMessage));

        return messages;
    }
    
    public static void ModelValidationErrorMessages(this ModelStateDictionary modelStateDictionary, out List<string> errorMessages)
    {
        var messages = modelStateDictionary.Values
            .SelectMany(x => x.Errors)
            .Select(x => x.ErrorMessage);

        errorMessages = messages.ToList();
    }
    
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState) 
    {
        if (result.ErrorMessage != null) 
        {
            // foreach (var error in result.Errors) 
            // {
                //modelState.AddModelError(error.PropertyName, error.ErrorMessage);
           // }
        }
    }
    
    public static void RegisterResourceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var policyHandler = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
      //  policyHandler.
    }
    
    public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex, int pageSize) where T : class
    {
        return new PagedList<T>(query, pageIndex, pageSize);
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize, CancellationToken cancellationToken = default) where T : class
    {
        var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await query.Skip((pageIndex > 0) ? (pageIndex - 1) * pageSize : 0).Take(pageSize).ToListAsync<T>(cancellationToken).ConfigureAwait(false);

        var pagedList = new PagedList<T>()
        {
            CurrentPage = pageIndex,
            PageSize = pageSize,
            TotalCount = count,
            Data = items,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        return pagedList;
    }

    public static PagedList<TDestination> ToPagedList<TDestination, TSource>(this IMapper mapper, PagedList<TSource> source) where TSource : class where TDestination : class
    {
        var data = mapper.Map<List<TDestination>>(source.Data);

        return new PagedList<TDestination>()
        {
            Data = data,
            CurrentPage = source.CurrentPage,
            PageSize = source.PageSize,
            TotalCount = source.TotalCount,
            TotalPages = source.TotalPages
        };
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IMovieService, MovieService>();
        services.AddTransient<IGenreService, GenreServices>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR((cfg) =>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddScoped<IDomainEventDispatcher, MediatrDomainEventDispatcher>();

        return services;
    }
}