using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LikeLinq
{
    public interface IQueryable<T> : IEnumerable<T>
    {
        Expression Expression { get; }
        IQueryProvider Provider { get; }

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> Select<TResult>(Expression<Func<T, TResult>> selector);
        IQueryable<T> OrderBy(Expression<Func<T, object>> keySelector);
        IQueryable<T> OrderByDescending(Expression<Func<T, object>> keySelector);
        IQueryable<T> GroupBy(Expression<Func<T, object>> keySelector);
        IQueryable<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector,
                                                   Expression<Func<TKey, IEnumerable<T>, TResult>> elementSelector);
        IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(IQueryable<TOuter> outer,
                                                             IQueryable<TInner> inner,
                                                             Expression<Func<T, TKey>> outerKeySelector,
                                                             Expression<Func<TOuter, TKey>> innerKeySelector,
                                                             Expression<Func<T, TOuter, TInner, TResult>> resultSelector);
        IQueryable<T> Take(int count);
        IQueryable<T> Skip(int count);
        int Count();
        bool Any();
        bool All(Expression<Func<T, bool>> predicate);
        // ... các phương thức khác
    }

    public interface IQueryProvider
    {
        IQueryable<T> CreateQuery<T>(Expression expression);
        object Execute(Expression expression);
    }
    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
