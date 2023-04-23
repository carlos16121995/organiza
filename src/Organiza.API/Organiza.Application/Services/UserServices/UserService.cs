// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Organiza.Application.Services.UserServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retorna usuário logado
        /// </summary>
        /// <returns></returns>
        ClaimsPrincipal User { get; }

        /// <summary>
        /// Retorna Id do usuário logado
        /// </summary>
        /// <returns></returns>
        string Name { get; }

        /// <summary>
        /// Recupera o valor de correlationId
        /// </summary>
        string CorrelationId { get; }

        /// <summary>
        /// Informa um novo valor ara correlationId
        /// </summary>
        /// <param name="correlationId"></param>
        void SetCorrelationId(string correlationId);

        /// <summary>
        /// Indicador se o usuário esta logado ou não
        /// </summary>
        /// <returns></returns>
        bool IsLogged();
    }

    /// <summary>
    /// Retorna usuário logado
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor accessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        public UserService(IHttpContextAccessor accessor) => this.accessor = accessor;

        /// <summary>
        /// Retorna usuário logado
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal User => accessor?.HttpContext?.User!;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Name => User?.Identity?.Name ?? string.Empty;

        /// <summary>
        /// Identificador para relacionar logs entre aplicações
        /// </summary>
        public string CorrelationId { get; private set; } = string.Empty;


        /// <summary>
        /// Método para setar um novo correlationId
        /// </summary>
        /// <param name="correlationId"></param>
        public void SetCorrelationId(string correlationId) => CorrelationId = correlationId;

        /// <summary>
        /// Indicador se o usuário esta logado ou não
        /// </summary>
        /// <returns></returns>
        public bool IsLogged() => string.IsNullOrWhiteSpace(Name);
    }
}
