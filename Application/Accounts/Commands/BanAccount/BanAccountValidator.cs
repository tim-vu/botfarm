using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace FORFarm.Application.Accounts.Commands.BanAccount
{
    public class BanAccountValidator : AbstractValidator<BanAccount>
    {
        public BanAccountValidator()
        {
            RuleFor(b => b.ID).NotEmpty();
        }
    }
}
