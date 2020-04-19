using FluentValidation;

namespace FORFarm.Application.Common.Validation
{
    public static class AccountPropertyValidators
    {

        public static IRuleBuilderOptions<T, string> ValidCredential<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().MinimumLength(3);
        }

    }
}
