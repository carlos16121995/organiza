using AspectCore.DynamicProxy;
using System.Diagnostics.CodeAnalysis;

namespace Organiza.API.Configurations.Filters.Interceptors
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class InterceptorBaseAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextReturnValue"></param>
        /// <returns></returns>
        public object? GetReturnValue(object contextReturnValue)
        {
            object? returnValue = null;
            if (contextReturnValue != null)
            {
                var generics = contextReturnValue.GetType().GetGenericArguments();
                generics = generics.Where((soruce, index) => index != generics.Length - 1).ToArray();
                returnValue = contextReturnValue switch
                {
                    Task value when value.GetType().IsGenericType => typeof(Task<>).MakeGenericType(generics).GetProperty("Result")?.GetValue(value),
                    Task value when !value.GetType().IsGenericType => null,
                    object value when value.GetType() != typeof(void) => value,
                    _ => null,
                };
            }

            return returnValue;
        }
    }
}
