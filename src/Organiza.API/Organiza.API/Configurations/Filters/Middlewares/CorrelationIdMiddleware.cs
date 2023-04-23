using System.Diagnostics.CodeAnalysis;

namespace Organiza.API.Configurations.Filters.Middlewares
{
    using Organiza.Application.Services.UserServices;
    using Organiza.Infrastructure.CrossCutting.Extensions;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userService"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            userService.SetCorrelationId(httpContextAccessor.GetCorrelationId());
            await _next(httpContext);
        }
    }
}
