using FluentValidation;

namespace FORFarm.Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountValidator : AbstractValidator<DeleteAccount>
    {
        public DeleteAccountValidator()
        {
            RuleFor(d => d.ID).GreaterThan(0);
        }
    }
}