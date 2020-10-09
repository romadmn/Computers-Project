using ComputersApp.Application.DataTransferObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.Api.Validators
{
    public class TokenValidator : AbstractValidator<TokenDto>
    {
        public TokenValidator()
        {
            RuleFor(x => x.JWT)
                .NotNull();
                RuleFor(x => x.RefreshToken)
                .NotNull();
        }
    }
}
