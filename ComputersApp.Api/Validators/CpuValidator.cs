using ComputersApp.Application.DataTransferObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.Api.Validators
{
    public class CpuValidator : AbstractValidator<CpuDto>
    {
        public CpuValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .Length(1, 50);
            RuleFor(x => x.CorsAmount)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.Frequency)
                .NotEmpty()
                .NotNull();
        }
    }
}
