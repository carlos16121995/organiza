// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Organiza.Domain.Config;
using Organiza.Domain.Dtos.Auth;
using Organiza.Domain.Entities.Users;
using System.Security.Claims;
using System.Text;

namespace Organiza.Application.Features.Auth.Commands.Jwt.AccessToken
{
    /// <summary>
    /// 
    /// </summary>
    public static class AccessTokenCommandHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public static async Task<SecurityTokenDescriptor> AccessToken(this ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim("Token", "Access"));
            var claims = new ClaimsIdentity(
                identity: new AuthenticationDetailDto
                {
                    IsAuthenticated = true,
                    Name = user.Id.ToString()
                },
                claims: userClaims);

            return new()
            {
                Subject = claims,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Token.SecretKey)),
                    SecurityAlgorithms.HmacSha512Signature),
                Audience = Settings.Token.Audience,
                Issuer = Settings.Token.Issuer,
                Expires = DateTime.UtcNow.AddMilliseconds(21600000)
            };
        }
    }
}
