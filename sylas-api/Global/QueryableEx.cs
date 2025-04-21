using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace sylas_api.Global;

public static class QueryableEx
{
    public static IQueryable<T> Paged<T>(this IQueryable<T> query, Pagination? pagination, out PaginationMeta? meta){
        if (pagination is null || pagination.Limit <=0){
            meta = new(){
                Total = query.Count(),
            };
            meta.PageSize = meta.Total;
            return query;
        }

        meta = new(){
            Page = pagination.Page,
            Limit = pagination.Limit,
            Total = query.Count()
        };

        query = query.Skip((pagination.Page - 1) * pagination.Limit).Take(pagination.Limit);
        meta.PageSize = query.Count();
        
        return query;
    }

    public static IQueryable<T> Randomize<T>(this IQueryable<T> query, int count){
        return query.OrderBy(x => Guid.NewGuid()).Take(count);
    }

    public static IQueryable<T> Where<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> @where)
    {
        return condition ? query.Where(@where) : query;
    }

    public static IQueryable<T> Search<T>(
        this IQueryable<T> query,
        SearchQuery? search,
        params Expression<Func<T, string>>[] propertySelectors)
    {
        return (search is null || string.IsNullOrEmpty(search.Content) || string.IsNullOrWhiteSpace(search.Content)) ? query : query.WhereLike($"%{search.Content}%".ToLower(), true, propertySelectors);
    }

    public static IQueryable<TSource> OrderBy<TSource, TKey>(
        this IQueryable<TSource> query,
        OrderQuery? order,
        string column,
        Expression<Func<TSource, TKey>> orderby
        )
    {
        return (order is not null && order.OrderBy is not null && order.OrderBy.Equals(column, StringComparison.OrdinalIgnoreCase)) ? ((order.Order is not null && order.Order.Equals("desc", StringComparison.OrdinalIgnoreCase)) ? query.OrderByDescending(orderby) : query.OrderBy(orderby)) : query;
    }

    public static IQueryable<T> WhereLike<T>(
        this IQueryable<T> source,
        string pattern,
        bool ignoreCase = false,
        params Expression<Func<T, string>>[] propertySelectors)
    {
        if (propertySelectors == null || propertySelectors.Length == 0)
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? orExpression = null;

        foreach (var selector in propertySelectors)
        {
            // Accède à la propriété : u => u.Name -> x.Name
            var memberAccess = Expression.Invoke(selector, parameter);

            Expression? toSearch = memberAccess;
            Expression? likePattern = Expression.Constant(pattern);

            if (ignoreCase)
            {
                var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
                toSearch = Expression.Call(toSearch!, toLowerMethod);
                likePattern = Expression.Constant(pattern.ToLower());
            }

            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new[]
            {
                typeof(DbFunctions), typeof(string), typeof(string)
            })!;

            var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
            var likeCall = Expression.Call(likeMethod, efFunctions, toSearch!, likePattern);

            orExpression = orExpression == null ? likeCall : Expression.OrElse(orExpression, likeCall);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(orExpression!, parameter);
        return source.Where(lambda);
    }

    public class Pagination{
        public int Page {
            get => _page;
            set{
                _page = (value <= 0)? 1 : value;
            }
        }
        private int _page = 1;
        public int Limit {get; set;} = -1; // Must be -1 as it's used to not filter by default

        //Actual count element sent in page
        public int PageSize {
            get; set;
        }
        public int Total {get; set;} = 0;

        public int PageCount {
            get{
                if(Limit <= 0 || Total <= 0)
                    return 1;
                return (int)Math.Ceiling(Total / (double)Limit);
            }
        }
    }

    public class SearchQuery{
        public string? Content {get;set;} = null;
    }

    public class OrderQuery{
        public string? OrderBy { get; set; }
        public string? Order { get; set; } = "asc";
    }
}
