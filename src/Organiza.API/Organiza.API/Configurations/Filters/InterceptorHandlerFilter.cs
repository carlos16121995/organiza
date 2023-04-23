using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Organiza.Application.Services.UserServices;
using Organiza.Domain.Infra.Exceptions;
using Organiza.Infrastructure.CrossCutting.Extensions;
using Organiza.Infrastructure.CrossCutting.Extensions.JsonConverters;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;

namespace Organiza.API.Configurations.Filters
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InterceptorHandlerFilter : ControllerBase, IAsyncExceptionFilter, IAsyncActionFilter
    {

        private object? BodyRequest { get; set; }
        private IUserService UserService { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public InterceptorHandlerFilter(IUserService userService) => UserService = userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var typeAction = context.ActionDescriptor.GetType();
            var controllerInfo = typeAction.GetProperty("ControllerTypeInfo")!.GetValue(context.ActionDescriptor) as TypeInfo;
            var actionName = typeAction.GetProperty("ActionName")!.GetValue(context.ActionDescriptor) as string;
            var method = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path.Value;
            BodyRequest = await GetObjectRequestByContext(context);
            Console.WriteLine($"[Organiza][{controllerInfo!.Name}][{actionName}] {method} {path} {LogExtensions.MontarLog("Request", BodyRequest)} {{CorrelationId}}", UserService.CorrelationId);
            var nextResult = await next();
            string logMessageResultBase = $"[Organiza][Result][{controllerInfo.Name}][{actionName}] {method} {path}";
            try
            {
                var bodyResponse = nextResult.Result?.GetType()?.GetProperty("Value")?.GetValue(nextResult.Result, null);

                if (bodyResponse != null)
                {
                    Console.WriteLine($"{logMessageResultBase} {LogExtensions.MontarLog("Result", bodyResponse)} {{CorrelationId}}", UserService.CorrelationId);
                }
                else
                {
                    Console.WriteLine($"{logMessageResultBase} {{CorrelationId}}", UserService.CorrelationId);
                }
            }
            catch
            {
                Console.WriteLine($"{logMessageResultBase} {LogExtensions.MontarLog("Request", BodyRequest)} {{CorrelationId}}", UserService.CorrelationId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var typeAction = context.ActionDescriptor.GetType();
            var controllerInfo = typeAction.GetProperty("ControllerTypeInfo")!.GetValue(context.ActionDescriptor) as TypeInfo;
            var actionName = typeAction.GetProperty("ActionName")!.GetValue(context.ActionDescriptor) as string;
            var method = context.HttpContext.Request.Method;

            var path = context.HttpContext.Request.Path.Value;

            var logBodyRequest = $"[Organiza][{controllerInfo!.Name}][{actionName}] {method} {path} {LogExtensions.MontarLog("Request", BodyRequest)} {{CorrelationId}}";

            var response = context.Exception switch
            {
                OrganizaException _ => context.Exception.BuildApiErrorMessage(UserService.CorrelationId, HttpStatusCode.BadRequest, logBodyRequest, UserService.CorrelationId, new { requestData = BodyRequest }),
                JsonSerializationException _ => context.Exception.BuildApiErrorMessage(UserService.CorrelationId, HttpStatusCode.BadRequest, $"[Organiza][{controllerInfo.Name}][{actionName}] {method} {path} (Request: Body nulo.) {{CorrelationId}}", UserService.CorrelationId),
                FluentValidation.ValidationException _ => context.Exception.BuildApiErrorMessage(UserService.CorrelationId, HttpStatusCode.BadRequest, logBodyRequest, UserService.CorrelationId, new { requestData = BodyRequest }),
                _ => context.Exception.BuildApiErrorMessage(UserService.CorrelationId, HttpStatusCode.InternalServerError, logBodyRequest, UserService.CorrelationId, new { requestData = BodyRequest })
            };

            context.Result = await Task.FromResult(response);
        }

        private static async Task<object?> GetObjectRequestByContext(FilterContext context)
        {
            context.HttpContext.Request.Body.Position = 0;
            using StreamReader reader = new(context.HttpContext.Request.Body);
            string text = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(text))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<object>(text, new AnonymizeJsonConverter());
            }
            catch
            {
                return null;
            }
        }
    }
}
