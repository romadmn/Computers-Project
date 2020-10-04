using ComputersApp.Application.DataTransferObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.Api.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(16)
                .Matches("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$").WithMessage("Invalid password.");
        }
    }
}
