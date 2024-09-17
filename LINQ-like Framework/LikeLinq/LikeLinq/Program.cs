using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LikeLinq
{
    // Interface IQueryable
    public interface IQueryable<T> : IEnumerable<T>
    {
        Expression Expression { get; }
        IQueryProvider Provider { get; }

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
        IQueryable<T> OrderBy(Expression<Func<T, object>> keySelector);
        IQueryable<T> OrderByDescending(Expression<Func<T, object>> keySelector);
        // Other methods can be added as necessary
    }

    // Interface IQueryProvider
    public interface IQueryProvider
    {
        IQueryable<T> CreateQuery<T>(Expression expression);
        object Execute(Expression expression);
    }

    // QueryProvider Implementation
    public class QueryProvider : IQueryProvider
    {
        private IEnumerable _data;

        public QueryProvider(IEnumerable data)
        {
            _data = data;
        }

        public IQueryable<T> CreateQuery<T>(Expression expression)
        {
            return new QueryableArray<T>((IEnumerable<T>)_data, expression);
        }

        public object Execute(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return _data;
            }

            var methodName = methodCallExpression.Method.Name;
            switch (methodName)
            {
                case "Where":
                    return ExecuteWhere(methodCallExpression);
                case "Select":
                    return ExecuteSelect(methodCallExpression);
                case "OrderBy":
                    return ExecuteOrderBy(methodCallExpression, false);
                case "OrderByDescending":
                    return ExecuteOrderBy(methodCallExpression, true);
                default:
                    throw new NotImplementedException($"Method '{methodName}' is not implemented.");
            }

        }

        // Execute Where
        private IEnumerable ExecuteWhere(MethodCallExpression expression)
        {
            var lambda = (LambdaExpression)((UnaryExpression)expression.Arguments[1]).Operand;
            var compiledLambda = (Func<object, bool>)lambda.Compile();
            var list = new List<object>();

            foreach (var item in _data)
            {
                if (compiledLambda(item))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        // Execute Select
        private IEnumerable ExecuteSelect(MethodCallExpression expression)
        {
            var lambda = (LambdaExpression)((UnaryExpression)expression.Arguments[1]).Operand;
            var compiledLambda = lambda.Compile();
            var list = new List<object>();

            foreach (var item in _data)
            {
                list.Add(compiledLambda.DynamicInvoke(item));
            }

            return list;
        }

        // Execute OrderBy/OrderByDescending
        private IEnumerable ExecuteOrderBy(MethodCallExpression expression, bool descending)
        {
            var lambda = (LambdaExpression)((UnaryExpression)expression.Arguments[1]).Operand;
            var compiledLambda = lambda.Compile();
            var list = new List<object>(_data.Cast<object>());

            list.Sort((x, y) =>
            {
                var xValue = compiledLambda.DynamicInvoke(x);
                var yValue = compiledLambda.DynamicInvoke(y);

                return descending ? Comparer<object>.Default.Compare(yValue, xValue)
                                  : Comparer<object>.Default.Compare(xValue, yValue);
            });

            return list;
        }
    }

    // QueryableArray Implementation
    public class QueryableArray<T> : IQueryable<T>
    {
        private IEnumerable<T> _data;
        private Expression _expression;
        private IQueryProvider _provider;

        public QueryableArray(IEnumerable<T> data)
        {
            _data = data;
            _expression = Expression.Constant(this);
            _provider = new QueryProvider(_data);
        }

        public QueryableArray(IEnumerable<T> data, Expression expression)
        {
            _data = data;
            _expression = expression;
            _provider = new QueryProvider(_data);
        }

        public Expression Expression => _expression;
        public IQueryProvider Provider => _provider;

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            var newExpression = Expression.Call(
                typeof(QueryableArray<T>),
                nameof(Where),
                new Type[] { typeof(T) },
                _expression,
                predicate
            );

            return new QueryableArray<T>(_data, newExpression);
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            var newExpression = Expression.Call(
                typeof(QueryableArray<T>),
                nameof(Select),
                new Type[] { typeof(T), typeof(TResult) },
                _expression,
                selector
            );

            return new QueryableArray<TResult>((IEnumerable<TResult>)_data, newExpression);
        }

        public IQueryable<T> OrderBy(Expression<Func<T, object>> keySelector)
        {
            var newExpression = Expression.Call(
                typeof(QueryableArray<T>),
                nameof(OrderBy),
                new Type[] { typeof(T) },
                _expression,
                keySelector
            );

            return new QueryableArray<T>(_data, newExpression);
        }

        public IQueryable<T> OrderByDescending(Expression<Func<T, object>> keySelector)
        {
            var newExpression = Expression.Call(
                typeof(QueryableArray<T>),
                nameof(OrderByDescending),
                new Type[] { typeof(T) },
                _expression,
                keySelector
            );

            return new QueryableArray<T>(_data, newExpression);
        }
    }

    // Class Person
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
