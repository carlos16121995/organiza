// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using Organiza.Application.Features.Auth.Commands.Logins;

namespace Organiza.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenCommandResponse : LoginCommandResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenExpiresIn"></param>
        /// <param name="refreshToken"></param>
        /// <param name="refreshTokenExpiresIn"></param>
        /// <param name="addressRegistered"></param>
        /// <param name="bankAccountRegistered"></param>
        public RefreshTokenCommandResponse(string token, DateTime tokenExpiresIn, string refreshToken, DateTime refreshTokenExpiresIn)
            : base(token, tokenExpiresIn, refreshToken, refreshTokenExpiresIn)
        {
        }
    }
}
