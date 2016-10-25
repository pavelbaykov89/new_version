using System;
using System.Linq;
using System.Linq.Expressions;

namespace SLK.Domain.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                source = source.Where(predicate);

            return source;
        }

        public static IQueryable<TSource> WhereIfText<TSource>(this IQueryable<TSource> source, string text, Expression<Func<TSource, bool>> predicate)
        {
            return source.WhereIf(!string.IsNullOrEmpty(text), predicate);
        }
    }
}
