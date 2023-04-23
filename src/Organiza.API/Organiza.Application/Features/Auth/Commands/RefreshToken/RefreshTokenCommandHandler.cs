// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Organiza.Application.Features.Auth.Commands.Jwt.AccessToken;
using Organiza.Application.Features.Auth.Commands.Jwt.HashToken;
using Organiza.Application.Features.Auth.Commands.Jwt.RefreshToken;
using Organiza.Domain.Config;
using Organiza.Domain.Entities.Users;
using Organiza.Domain.Infra.Exceptions;
using Organiza.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace Organiza.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Context _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RefreshTokenCommandHandler(UserManager<ApplicationUser> userManager, Context context)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _tokenHandler.ValidateToken(
                request.RefreshToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Settings.Token.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                },
                out var refreshTokenSecure);


            if (!refreshToken.Claims.Any(claim => claim.Value.Equals("Refresh")))
                throw new OrganizaException("Falha ao revalidar token.", HttpStatusCode.BadRequest);

            var user = await _context.ApplicationUsers.Where(user => user.Id
                                                                    .Equals(new Guid(refreshToken.Identity!.Name!)))
                                                    .FirstAsync(cancellationToken);
            var newAccessToken = await user.AccessToken(_userManager);
            var newRefreshToken = user.RefreshToken();

            return new(newAccessToken.HashToken(), (DateTime)newAccessToken.Expires!, newRefreshToken.HashToken(), (DateTime)newRefreshToken.Expires!);
        }
    }
}
