using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tsi.EntityFrameworkCore.Respositories.Extensions
{
    public static class QueryableExtensions
    {
        public static Task<T>? GetById<T>(this IQueryable<T> query, Guid id)
        {
            var parameter = Expression.Parameter(typeof(T));
            var left = Expression.Property(parameter, "Id");
            var right = Expression.Constant(id);
            var equal = Expression.Equal(left, right);
            var byId = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return query.SingleAsync(byId);
        }
    }
}
