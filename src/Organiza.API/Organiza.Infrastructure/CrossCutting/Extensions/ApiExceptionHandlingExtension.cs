using Microsoft.AspNetCore.Mvc;
using Organiza.Domain.Infra.BaseResponses;
using Organiza.Domain.Infra.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{

    /// <summary>
    /// Classe para padronização de retornos de erros
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApiExceptionHandlingExtension
    {
        private const string UNKNOWN_ERROR_MESSAGE = "Ocorreu um erro inesperado ao processar a sua solicitação. Verifique os dados e tente novamente.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="correlationId"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static IActionResult BuildApiErrorMessage(this Exception exception, string correlationId, HttpStatusCode? statusCode = default)
        {
            if (statusCode is null)
            {
                throw new ArgumentNullException(nameof(statusCode));
            }

            var baseResponse = new BaseResponse<string>();
            if (exception is OrganizaException)
            {
                OrganizaException adminException = exception as OrganizaException ?? new();
                baseResponse.Message = adminException.Message;
            }
            else
                baseResponse.Message = $"{exception.FullMessage().Replace("Validation failed: \r\n -- : ", string.Empty)}";

            baseResponse.Errors = exception is FluentValidation.ValidationException vex ? ErrorResponse(vex) : Enumerable.Empty<ErrorModel>();
            baseResponse.Source = $"{exception.Source ?? UNKNOWN_ERROR_MESSAGE}";
            baseResponse.StatusCode = statusCode is null ? exception.GetStatusCode() : statusCode;
            baseResponse.CorrelationId = correlationId;

            return new ObjectResult(baseResponse)
            {
                StatusCode = (int)baseResponse.StatusCode,
                DeclaredType = exception.GetType()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="correlationId"></param>
        /// <param name="statusCode"></param>
        /// <param name="logMessage"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult BuildApiErrorMessage(this Exception exception, string correlationId, HttpStatusCode statusCode, string logMessage, params object[] data)
        {
            Console.WriteLine(exception + logMessage + data);
            return exception.BuildApiErrorMessage(correlationId, statusCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="correlationId"></param>
        /// <param name="logMessage"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult BuildApiErrorMessage(this Exception exception, string correlationId, string logMessage, params object[] data)
        {
            Console.WriteLine(exception + logMessage + data);
            return exception.BuildApiErrorMessage(correlationId);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="correlationId"></param>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        public static IActionResult BuildApiErrorMessage(this Exception exception, string correlationId, string logMessage)
        {
            Console.WriteLine(exception + $"{logMessage} Error: {exception.FullMessage()}");
            return exception.BuildApiErrorMessage(correlationId);
        }

        private static IEnumerable<ErrorModel> ErrorResponse(FluentValidation.ValidationException ex)
            => ex.Errors
                .GroupBy((vf) => vf.PropertyName, (key, vf) => new ErrorModel
                {
                    Property = key,
                    Message = vf.Select((e) => e.ErrorMessage).ToList(),
                })
                .ToList();

        private static HttpStatusCode GetStatusCode(this Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            string typeName = exception is AggregateException ?
                exception.InnerException!.GetType().Name :
                exception.GetType().Name;

            switch (typeName)
            {
                case "ValidationException":
                case "JsonSerializationException":
                    {
                        httpStatusCode = HttpStatusCode.BadRequest;
                        break;
                    }
                case "OrganizaException":
                    {
                        httpStatusCode = exception is AggregateException ?
                            (exception.InnerException as OrganizaException)!.StatusCode :
                            (exception as OrganizaException)!.StatusCode;
                        break;
                    }
                default:
                    break;
            }

            return httpStatusCode;
        }
    }
}
