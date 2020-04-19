using System;
using FluentValidation;
using FORFarm.Application.Common.Validation;

namespace FORFarm.Application.Bots.Commands.RegisterBot
{
    public class RegisterBotValidator : AbstractValidator<RegisterBot>
    {
        public RegisterBotValidator()
        {
            RuleFor(r => r.Username).ValidCredential();
            RuleFor(r => r.Tag).NotEqual(Guid.Empty);
        }
    }
}