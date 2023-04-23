// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using MediatR;

namespace Organiza.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenCommand : IRequest<RefreshTokenCommandResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
