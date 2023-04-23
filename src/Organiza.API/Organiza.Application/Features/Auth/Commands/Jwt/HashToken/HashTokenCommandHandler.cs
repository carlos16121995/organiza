// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Organiza.Application.Features.Auth.Commands.Jwt.HashToken
{
    /// <summary>
    /// 
    /// </summary>
    public static class HashTokenCommandHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenDescriptor"></param>
        /// <returns></returns>
        public static string HashToken(this SecurityTokenDescriptor tokenDescriptor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
