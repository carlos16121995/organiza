using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using MongoDB.Bson.Serialization;

using System.Diagnostics.CodeAnalysis;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class UtilExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static string GetCorrelationId(this IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("x-correlation-id", out StringValues value)
                || httpContextAccessor.HttpContext.Request.Headers.TryGetValue("CorrelationId", out value)
                || httpContextAccessor.HttpContext.Request.Headers.TryGetValue("correalationId", out value))
                return value.ToString();


            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryGetCollection<T>(this string value, out T data)
        {
            bool sucess = false;

            try
            {
                data = BsonSerializer.Deserialize<T>(value);
                sucess = true;
            }
            catch
            {
                data = default;
            }

            return sucess;
        }
    }
}
