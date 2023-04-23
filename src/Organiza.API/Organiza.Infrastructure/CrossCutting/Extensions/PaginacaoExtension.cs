using Organiza.Domain.Infra.BaseRequests;
using Organiza.Domain.Infra.BaseResponses;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class PaginacaoExtension
    {
        public static TResponse GetPagedList<TResponse, T>(this IQueryable<T> query, PagedBaseRequest request) where TResponse : PagedBaseResponse<T>, new()
        {
            var response = new TResponse();
            var count = query.Count();

            if (!string.IsNullOrEmpty(request.OrderByProperty))
            {
                if (request.OrderByProperty.StartsWith("-"))
                    query = query.OrderByPropertyDescending(request.OrderByProperty[1..]);
                else
                    query = query.OrderByProperty(request.OrderByProperty);
            }

            response.TotalPages = (int)Math.Round((decimal)count / request.Size, mode: MidpointRounding.ToPositiveInfinity);
            response.TotalRegisters = count;
            response.Data = query
                                .Skip((request.Page - 1) * request.Size)
                                .Take(request.Size)
                                .ToList();
            return response;
        }



        private static readonly MethodInfo OrderByMethod =
            typeof(Queryable).GetMethods()
                .Single(method => method.Name == "OrderBy" && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByDescendingMethod =
            typeof(Queryable).GetMethods()
                .Single(method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> query, string propertyName)
        {
            if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return query;
            }
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
            Expression orderByProperty = Expression.Property(parameterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, parameterExpression);
            MethodInfo genericMethod = OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
            object ret = genericMethod.Invoke(null, new object[] { query, lambda })!;
            return (IQueryable<T>)ret!;
        }

        public static IQueryable<T> OrderByPropertyDescending<T>(this IQueryable<T> query, string propertyName)
        {
            if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return query;
            }
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
            Expression orderByProperty = Expression.Property(parameterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, parameterExpression);
            MethodInfo genericMethod = OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
            object ret = genericMethod.Invoke(null, new object[] { query, lambda })!;
            return (IQueryable<T>)ret!;
        }
    }
}
