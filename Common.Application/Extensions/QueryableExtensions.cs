using System.Linq.Expressions;
using Common.Application.Pagination;
using Common.Domain.Filter;
using Microsoft.EntityFrameworkCore;

namespace Common.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<Page<T>> ToPageAsync<T>(
        this IQueryable<T> source,
        FilterData filter,
        CancellationToken cancellationToken
    )
    {
        if (filter.PageNumber < 1)
            throw new ArgumentException("PageNumber cannot be less than 1");
        if (filter.PageSize < 1)
            throw new ArgumentException("PageSize cannot be less than 1");

        if (filter.SortField is not null)
        {
            source = source.OrderByProperty(
                filter.SortField, 
                filter.SortDirection is not null && filter.SortDirection.ToLower().Equals("desc"));
        }
        
        int totalItems = await source.CountAsync(cancellationToken: cancellationToken);
        List<T> items = await source
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new Page<T>(items, filter.PageNumber, filter.PageSize, totalItems);
    }

    private static IQueryable<T> OrderByProperty<T>(
        this IQueryable<T> source,
        string fieldName,
        bool descending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, fieldName);
        var orderByExpression = Expression.Lambda(property, parameter);
        var method = descending ? "OrderByDescending" : "OrderBy";
        var result = Expression.Call(
            typeof(Queryable), 
            method,
            [typeof(T), property.Type], 
            source.Expression, 
            orderByExpression);
        
        return source.Provider.CreateQuery<T>(result);
    }
}