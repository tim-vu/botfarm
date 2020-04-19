using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using FORFarm.Application.Common.Validation;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountValidator : AbstractValidator<UpdateAccount>
    {
        public UpdateAccountValidator()
        {
            RuleFor(x => x.Password).ValidCredential();
        }
    }
}
