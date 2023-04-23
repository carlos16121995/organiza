using Organiza.Domain.Dtos.Infra.BaseClients;
using Organiza.Domain.Infra.Exceptions;

using System.Net.Http;

namespace Organiza.Infrastructure.BaseClients
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseClientService : IBaseClientService
    {
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseClientService(IHttpClientFactory clientFactory) => _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OrganizaException"></exception>
        public async Task<HttpResponseMessage> SendAsync(BaseClientRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient(request.Client);

                using var req = new HttpRequestMessage(request.HttpMethod, request.Path);

                if (request.Content is not null)
                    req.Content = request.Content;

                if (request!.Headers != null && request!.Headers.Any())
                    foreach (var kvp in request.Headers)
                        req.Headers.Add(kvp.Key, kvp.Value);

                return await httpClient.SendAsync(req, cancellationToken);
            }
            catch (Exception ex) { throw new OrganizaException($"Falha ao se comunicar com {request.Client}", ex); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="httpResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OrganizaException"></exception>
        public async Task ResultVoid(string msg, HttpResponseMessage httpResponse, CancellationToken cancellationToken)
        {
            if (httpResponse.IsSuccessStatusCode)
                return;

            var res = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            throw new OrganizaException(msg + res);
        }
    }
}
