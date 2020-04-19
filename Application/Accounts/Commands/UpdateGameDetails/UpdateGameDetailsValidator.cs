using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FORFarm.Domain.Enums;

namespace FORFarm.Application.Accounts.Commands.UpdateGameDetails
{
    public class UpdateGameDetailsValidator : AbstractValidator<UpdateGameDetails>
    {
        public UpdateGameDetailsValidator()
        {
            RuleFor(x => x.ID).NotEmpty();
            RuleFor(x => x.MembershipDaysRemaining).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Skills).Must(AreSkillsValid)
                .When(x => x.Skills != null);
        }

        public bool AreSkillsValid(IReadOnlyDictionary<SkillType, int> skills)
        {
            return skills.Count == Enum.GetValues(typeof(SkillType)).Length &&
                skills.Values.All(v => v >= 1 && v <= 99) &&
                skills[SkillType.Hitpoints] >= 10;
        }
    }
}
