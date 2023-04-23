using AspectCore.DynamicProxy;
using Organiza.Infrastructure.CrossCutting.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Organiza.API.Configurations.Filters.Interceptors
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggerInterceptorAttribute : InterceptorBaseAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type? ParameterValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            object? request = ParameterValue != null ? context.Parameters.FirstOrDefault(o => o.GetType() == ParameterValue) : null;
            try
            {
                Console.WriteLine($"[Organiza][{context.ImplementationMethod.Name}] {LogExtensions.MontarLog("Request", request)}");
                await next(context);
                object? returnValue = GetReturnValue(context.ReturnValue);
                Console.WriteLine($"[Organiza][{context.ImplementationMethod.Name}] {LogExtensions.MontarLog("Result", returnValue)}");
            }
            catch (Exception ex)
            {
                ex.BuildApiErrorMessage($"[Organiza][{context.ImplementationMethod.Name}] Erro! {LogExtensions.MontarLog("Request", request)}");
                throw;
            }
        }
    }
}
