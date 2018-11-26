using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Simple.Repositories.Base
{
    public static class WhereIfExtension
    {
        public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
        public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, int, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
        public static IEnumerable<TEntity> WhereIf<TEntity>(this IEnumerable<TEntity> source, bool condition, Func<TEntity, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
        public static IEnumerable<TEntity> WhereIf<TEntity>(this IEnumerable<TEntity> source, bool condition, Func<TEntity, int, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
