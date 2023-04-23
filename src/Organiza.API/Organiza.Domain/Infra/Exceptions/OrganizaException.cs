using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;

namespace Organiza.Domain.Infra.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class OrganizaException : Exception
    {
        /// <summary>
        /// Codigo de retorno.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; } = HttpStatusCode.BadRequest;

        /// <summary>
        /// Detalhes da exceção.
        /// </summary>
        public string Details { get; private set; } = string.Empty;

        /// <summary>
        /// Recuperável?
        /// </summary>
        public bool Recoverable { get; private set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public override string Message { get; } = string.Empty;

        /// <summary>
        /// Exceção simples com mensagem.
        /// </summary>
        public OrganizaException()
            : this(message: "Ocorreu um erro inesperado ao processar a sua solicitação.") { }

        /// <summary>
        /// Exceção simples com uma mensagem personalizada e também com a mensagem da exceção.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public OrganizaException(string message, Exception? innerException = null)
            : this(message, details: string.Empty, innerException) { }

        /// <summary>
        /// Exceção personalizada com a mensagem, o código e a innerException
        /// </summary>
        public OrganizaException(string message, HttpStatusCode codResponse, Exception? innerException = null)
            : this(message, codResponse, details: string.Empty, innerException) { }

        /// <summary>
        /// Exceção personalizada com a mensagem, o código, os detalhes e a innerException
        /// </summary>
        public OrganizaException(string message, HttpStatusCode codResponse, string details, Exception? innerException = null)
            : this(message, codResponse, recoverable: false, details, innerException) { }

        /// <summary>
        /// Exceção personalizada com a mensagem, o código, se será recuperavel, os detalhes e a innerException
        /// </summary>
        public OrganizaException(string message, HttpStatusCode codResponse, bool recoverable, string details, Exception? innerException = null)
            : this(message, details, innerException)
        {
            StatusCode = codResponse;
            Recoverable = recoverable;
        }

        /// <summary>
        /// Exceção personalizada com a mensagem, se será recuperavel e a innerException
        /// </summary>
        public OrganizaException(string message, bool recoverable, Exception? innerException = null)
            : this(message, recoverable, string.Empty, innerException) { }

        /// <summary>
        /// Exceção personalizada com a mensagem, se será recuperavel, os detalhes e a innerException
        /// </summary>
        public OrganizaException(string message, bool recoverable, string details, Exception? innerException = null)
            : this(message, codResponse: HttpStatusCode.BadRequest, recoverable, details, innerException) { }


        /// <summary>
        /// Exceção personalizada com a mensagem, os detalhes e a innerException
        /// </summary>
        public OrganizaException(string message, string details, Exception? innerException = null)
          : base(message ?? "Ocorreu um erro inesperado ao processar a sua solicitação.", innerException)
        {
            Details = string.IsNullOrWhiteSpace(details) ? "" : details;
            Message = string.IsNullOrWhiteSpace(message) ? "" : message;
        }

        private OrganizaException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
            Details = string.Empty;
            Message = string.Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
    }
}
