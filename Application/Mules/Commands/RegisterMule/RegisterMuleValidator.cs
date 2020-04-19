using FluentValidation;
using FORFarm.Application.Common.Validation;

namespace FORFarm.Application.Mules.Commands.RegisterMule
{
    public class RegisterMuleValidator : AbstractValidator<RegisterMule>
    {
        public RegisterMuleValidator()
        {
            RuleFor(r => r.Username).ValidCredential();
            RuleFor(r => r.DisplayName).ValidCredential();
            RuleFor(r => r.Tag).NotEmpty();
        }
    }
}