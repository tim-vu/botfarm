using FluentValidation;

namespace FORFarm.Application.Bots.Commands.FinishMuling
{
    public class FinishMulingValidator : AbstractValidator<FinishMuling>
    {
        public FinishMulingValidator()
        {
            RuleFor(f => f.Tag).NotEmpty();
        }
    }
}