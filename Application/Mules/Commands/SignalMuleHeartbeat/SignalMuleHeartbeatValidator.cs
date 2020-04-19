using FluentValidation;

namespace FORFarm.Application.Mules.Commands.SignalMuleHeartbeat
{
    public class SignalMuleHeartbeatValidator : AbstractValidator<SignalMuleHeartbeat>
    {
        public SignalMuleHeartbeatValidator()
        {
            RuleFor(h => h.Tag).NotEmpty();
            RuleFor(h => h.Gold).GreaterThanOrEqualTo(0);
        }
    }
}