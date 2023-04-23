using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Organiza.Infrastructure.Persistence;

namespace Organiza.Application.Features.Auth.Commands.Logins
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator(Context _context)
        {
            RuleFor(command => command.Email)
               .MustAsync(async (_, email, cancellation) => await _context.ApplicationUsers.AnyAsync(usuario => usuario.Active && usuario.Email == email, cancellation))
               .WithMessage("Usuário e/ou Senha inválido.")
               .MaximumLength(256)
               .NotEmpty();

            RuleFor(command => command.Password)
                .MaximumLength(32)
                .WithMessage("Usuário e/ou Senha inválido.")
                .NotEmpty();
        }
    }
}
