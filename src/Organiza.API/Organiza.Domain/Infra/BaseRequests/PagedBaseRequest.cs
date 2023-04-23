using System.Diagnostics.CodeAnalysis;

namespace Organiza.Domain.Infra.BaseRequests
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PagedBaseRequest
    {
        /// <summary>
        /// Página  a ser recuperada. Deafult: 1
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Quantos items serão retornados em cada página. Deafult: 10
        /// </summary>
        public int Size { get; set; } = 10;
        /// <summary>
        /// Propriedade que será utilizada para ordenação
        /// </summary>
        public string? OrderByProperty { get; set; } = "Id";
    }
}
