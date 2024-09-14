using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LikeLinq
{
    public interface IQueryable<T> : IEnumerable<T>
    {
        Expression Expression { get; }
        IQueryProvider Provider { get; }

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> Select<TResult>(Expression<Func<T, TResult>> selector);
        // ... các phương thức khác
    }
    public interface IQueryProvider
    {
        IQueryable<T> CreateQuery<T>(Expression expression);
        object Execute(Expression expression);
    }
}
