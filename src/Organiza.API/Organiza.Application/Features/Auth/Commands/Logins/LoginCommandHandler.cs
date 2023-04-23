using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Organiza.Application.Features.Auth.Commands.Jwt.AccessToken;
using Organiza.Application.Features.Auth.Commands.Jwt.HashToken;
using Organiza.Application.Features.Auth.Commands.Jwt.RefreshToken;
using Organiza.Domain.Entities.Users;
using Organiza.Domain.Infra.Exceptions;
using Organiza.Infrastructure.Persistence;
using System.Net;

namespace Organiza.Application.Features.Auth.Commands.Logins
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
    {
        private readonly Context _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoginCommandHandler(Context context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = (await _context.ApplicationUsers.FirstAsync(user => user.Email == request.Email, cancellationToken))!;

            SignInResult loginResult = await _signInManager.PasswordSignInAsync(
                    user,
                    request.Password,
                    false,
                    lockoutOnFailure: false);

            if (!loginResult.Succeeded)
                throw new OrganizaException("Usuario ou senha incorretos.", HttpStatusCode.BadRequest);

            var accessToken = await user.AccessToken(_userManager);
            var refreshToken = user.RefreshToken();

            return new(accessToken.HashToken(), (DateTime)accessToken.Expires!, refreshToken.HashToken(), (DateTime)refreshToken.Expires!);
        }
    }
}
