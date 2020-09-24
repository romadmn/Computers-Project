using ComputersApp.Application.DataTransferObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.Api.Validators
{
    public class ComputerValidator : AbstractValidator<ComputerDto>
    {
        public ComputerValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Name)
                .NotNull()
                .Length(1, 50);
            RuleFor(x => x.RamAmount)
                .NotNull();
            RuleFor(x => x.SsdAmount)
                .NotNull();
        }
    }
}
