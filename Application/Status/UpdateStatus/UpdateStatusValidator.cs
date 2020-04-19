using FluentValidation;
using FORFarm.Application.Common.Interfaces;

namespace FORFarm.Application.Status.UpdateStatus
{
    public class UpdateStatusValidator : AbstractValidator<UpdateStatus>
    {
        public UpdateStatusValidator(IStateService stateService)
        {
            RuleFor(u => u.Running).MustAsync(async (status, current, token) => await stateService.IsRunning() != current);
            RuleFor(u => u.Running).NotEqual(true)
                .WhenAsync(async (status, token) => !await stateService.CanStart());

        }
    }
}