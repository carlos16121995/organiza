using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Organiza.Domain.Dtos.Infra.BaseClients
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseClientRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="path"></param>
        /// <param name="httpMethod"></param>
        /// <param name="content"></param>
        /// <param name="headers"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseClientRequest(string client, string path, HttpMethod httpMethod, ByteArrayContent? content = null, Dictionary<string, string>? headers = null)
        {
            if (string.IsNullOrWhiteSpace(client))
                throw new ArgumentException($"'{nameof(client)}' não pode ser nulo nem espaço em branco.", nameof(client));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"'{nameof(path)}' não pode ser nulo nem espaço em branco.", nameof(path));

            Client = client;
            Path = path;
            HttpMethod = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));
            Content = content;
            Headers = headers;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Client { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;
        /// <summary>
        /// 
        /// </summary>
        public ByteArrayContent? Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string>? Headers { get; set; }
    }
}
