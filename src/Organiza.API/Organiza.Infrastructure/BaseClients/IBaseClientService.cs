using Organiza.Domain.Dtos.Infra.BaseClients;

namespace Organiza.Infrastructure.BaseClients
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseClientService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="httpResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ResultVoid(string msg, HttpResponseMessage httpResponse, CancellationToken cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> SendAsync(BaseClientRequest request, CancellationToken cancellationToken);
    }
}