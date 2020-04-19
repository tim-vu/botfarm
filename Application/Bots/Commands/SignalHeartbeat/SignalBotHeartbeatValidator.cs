using System;
using FluentValidation;

namespace FORFarm.Application.Bots.Commands.SignalHeartbeat
{
    public class SignalBotHeartbeatValidator : AbstractValidator<SignalBotHeartbeat>
    {
        public SignalBotHeartbeatValidator()
        {
            RuleFor(s => s.Tag).NotEqual(Guid.Empty);
        }
    }
}