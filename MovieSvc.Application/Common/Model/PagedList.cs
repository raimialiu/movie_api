using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MovieSvc.Application.Common.Model;

public class PagedList<T> where T : class
{
    public PagedList()
    {
    }

    public PagedList(IQueryable<T> query, int page, int pageSize)
    {
        CurrentPage = page;
        PageSize = pageSize;
        TotalCount = query.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        Data = query.Skip((page > 0) ? (page - 1) * pageSize : 0).Take(pageSize).ToList();
    }

    public PagedList(List<T> query, int page, int pageSize)
    {
        CurrentPage = page;
        PageSize = pageSize;
        TotalCount = query.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        Data = query.Skip((page > 0) ? (page - 1) * pageSize : 0).Take(pageSize).ToList();
    }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public List<T> Data { get; set; }
}

public static class PaginationExtension
{
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
}