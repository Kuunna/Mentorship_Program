using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LikeLinq
{
    public interface IMyQueryable<T>
    {
        Expression Expression { get; }
        IMyQueryProvider Provider { get; }
    }
    public interface IMyQueryProvider
    {
        IQueryable<T> CreateQuery<T>(Expression expression);
        object Execute(Expression expression);
    }
    public class MyQueryable<T> : IMyQueryable<T>
    {
        public Expression Expression { get; private set; }
        public IMyQueryProvider Provider { get; private set; }

        public MyQueryable(IMyQueryProvider provider, Expression expression)
        {
            Provider = provider;
            Expression = expression;
        }
    }

}
