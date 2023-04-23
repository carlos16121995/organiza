// Copyright (c) 2022, Vendi Porque Cresci™. All rights reserved.
// Copyright (c) 2022, Marttech Desenvolvimento de Software. All rights reserved.
// PRIVATE SOURCE. Any kind of unauthorized use is prohibited.

using FluentValidation;

namespace Organiza.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public RefreshTokenCommandValidator()
        {
            RuleFor(d => d.RefreshToken)
                .NotEmpty();
        }
    }
}
