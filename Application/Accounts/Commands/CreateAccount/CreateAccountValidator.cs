using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Accounts.Commands.CreateAccount
{
    class CreateAccountValidator : AbstractValidator<CreateAccount>
    {
        private IFarmContext _context;
        
        public CreateAccountValidator(IFarmContext context)
        {
            _context = context;
            RuleFor(x => x.Username).ValidCredential()
                .DependentRules(() =>
                    RuleFor(x => x.Username).MustAsync((un, ctx, ct) => UsernameIsFree(un.Username, ct)));

            RuleFor(x => x.Password).ValidCredential();
            RuleFor(x => x.RemainingMembershipDays).GreaterThanOrEqualTo(0);
        }

        private Task<bool> UsernameIsFree(string username, CancellationToken ctx)
        {
            return _context.Accounts.CountAsync(a => a.Username == username, ctx).ContinueWith(r => r.Result == 0);
        }

    }
}
