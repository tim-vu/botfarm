using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FORFarm.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Proxies.Commands.CreateProxy
{
    public class CreateProxyValidator : AbstractValidator<CreateProxy>
    {

        private readonly IFarmContext _context;

        public CreateProxyValidator(IFarmContext context)
        {
            _context = context;

            RuleFor(x => x.Ip).ValidIpAddress()
                .DependentRules(() =>
                    RuleFor(x => x.Ip).MustAsync(IsAddressAvailable));

            RuleFor(x => x.Port).ValidPort();

        }

        private Task<bool> IsAddressAvailable(string ip, CancellationToken ct)
        {
            return _context.Proxies.Where(p => p.Ip == ip).AnyAsync(ct).ContinueWith(t => !t.Result);
        }

    }
}
