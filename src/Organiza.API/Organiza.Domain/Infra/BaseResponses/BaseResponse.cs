using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;

namespace Organiza.Domain.Infra.BaseResponses
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    [ExcludeFromCodeCoverage]
    public class BaseResponse<TData> : BaseResponse
    {
        /// <summary>
        /// Objeto de retorno
        /// </summary>
        public virtual TData? Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseResponse()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        public BaseResponse(TData data, string message = "", HttpStatusCode? statusCode = null)
        {
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    [ExcludeFromCodeCoverage]
    public class PagedBaseResponse<TData> : BaseResponse
    {
        /// <summary>
        /// Objeto paginado
        /// </summary>
        public IEnumerable<TData>? Data { get; set; }

        /// <summary>
        /// Total de páginas disponíveis
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Quantidade total de registros
        /// </summary>
        public long TotalRegisters { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BaseResponse
    {
        /// <summary>
        /// Identificador da requisição. Utilizado para conferir logs e métricas
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Nome da referenência calsadora do erro
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Status code da requisição
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Lista de erros em caso de falha na requisição
        /// </summary>
        public IEnumerable<ErrorModel> Errors { get; set; } = new List<ErrorModel>();

        /// <summary>
        /// 
        /// </summary>
        public BaseResponse()
        {
            CorrelationId = string.Empty;
            Message = string.Empty;
            Source = string.Empty;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorModel
    {
        /// <summary>
        /// Propriedade com possível erro
        /// </summary>
        public string? Property { get; set; }

        /// <summary>
        /// Erros constatados
        /// </summary>
        public IEnumerable<string>? Message { get; set; }
    }
}
