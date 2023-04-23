using MediatR;

namespace Organiza.Application.Features.Auth.Commands.Logins
{
    public class LoginCommand : IRequest<LoginCommandResponse>
    {
        public LoginCommand(string email, string senha)
        {
            Email = email;
            Password = senha;
        }

        /// <summary>
        /// Email do usuário
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Password { get; set; }
    }
}
