// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using Microsoft.IdentityModel.Tokens;
using Organiza.Domain.Config;
using Organiza.Domain.Dtos.Auth;
using Organiza.Domain.Entities.Users;
using System.Security.Claims;
using System.Text;

namespace Organiza.Application.Features.Auth.Commands.Jwt.RefreshToken
{
    /// <summary>
    /// 
    /// </summary>
    public static class RefreshTokenCommandHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="VendiException"></exception>
        public static SecurityTokenDescriptor RefreshToken(this ApplicationUser user)
        {
            var claims = new ClaimsIdentity(
                identity: new AuthenticationDetailDto
                {
                    IsAuthenticated = true,
                    Name = user.Id.ToString()
                }, claims: new List<Claim>() { new Claim("Token", "Refresh") });

            return new()
            {
                Subject = claims,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Token.SecretKey)),
                    SecurityAlgorithms.HmacSha512Signature),
                Audience = Settings.Token.Audience,
                Issuer = Settings.Token.Issuer,
                Expires = DateTime.UtcNow.AddDays(7)
            };
        }
    }
}
