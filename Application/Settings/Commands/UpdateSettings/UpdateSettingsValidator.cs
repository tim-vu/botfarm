using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace FORFarm.Application.Settings.Commands.UpdateSettings
{
    public class UpdateSettingsValidator : AbstractValidator<UpdateSettings>
    {
        public UpdateSettingsValidator()
        {
            RuleFor(u => u.Settings.LaunchSleep).GreaterThanOrEqualTo(0);
            RuleFor(u => u.Settings.MaxActiveBots).GreaterThan(0);
            RuleFor(u => u.Settings.MinActiveMules).GreaterThan(0);
            RuleFor(u => u.Settings.MaxActiveMules).GreaterThanOrEqualTo(u => u.Settings.MinActiveMules);
            RuleFor(u => u.Settings.ConcurrentAccountsPerProxy).GreaterThan(0);
            RuleFor(u => u.Settings.MaxAccountsPerProxy).GreaterThan(0)
                .DependentRules(() =>
                    RuleFor(u => u.Settings.MaxAccountsPerProxy).Must((m, f) => f >= m.Settings.ConcurrentAccountsPerProxy));
            RuleFor(u => u.Settings.MuleIntervalMinutes).GreaterThanOrEqualTo(30);
        }
    }
}
