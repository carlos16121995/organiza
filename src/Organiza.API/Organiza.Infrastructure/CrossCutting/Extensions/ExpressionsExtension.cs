﻿using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class PredicateComposer
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new
            {
                f,
                s = second.Parameters[i]
            }).ToDictionary(p => p.s, p => p.f);
            Expression arg = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, arg), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            => first.Compose(second, new Func<Expression, Expression, Expression>(Expression.And));

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
            => first.Compose(second, new Func<Expression, Expression, Expression>(Expression.Or));
    }
    [ExcludeFromCodeCoverage]
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            => this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            => new ParameterRebinder(map).Visit(exp);

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (map.TryGetValue(p, out var value))
                p = value;

            return base.VisitParameter(p);
        }
    }
}
