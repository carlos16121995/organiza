namespace Organiza.Application.Features.Auth.Commands.Logins
{
    public class LoginCommandResponse
    {
        public LoginCommandResponse(string token, DateTime tokenExpiresIn, string refreshToken, DateTime refreshTokenExpiresIn)
        {
            Token = token;
            TokenExpiresIn = tokenExpiresIn;
            RefreshToken = refreshToken;
            RefreshTokenExpiresIn = refreshTokenExpiresIn;
        }

        /// <summary>
        /// Token para autenticação
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expiração do token
        /// </summary>
        public DateTime TokenExpiresIn { get; set; }

        /// <summary>
        /// Chave para realizar refreshToken
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Data de Expiração do refreshToken
        /// </summary>
        public DateTime RefreshTokenExpiresIn { get; set; }
    }
}
