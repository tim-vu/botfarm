using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Accounts.Commands.BanAccount
{
    public class BanAccountHandler : IRequestHandler<BanAccount>
    {
        private readonly IFarmContext _context;

        public BanAccountHandler(IFarmContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BanAccount request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Include(a => a.Proxy)
                    .ThenInclude(p => p.ActiveAccounts)
                .FirstOrDefaultAsync(a => a.ID == request.ID, cancellationToken);

            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.ID);
            }

            account.Banned = true;

            if (account.Proxy != null)
            {
                account.Proxy.ActiveAccounts.Remove(account);

                if (account.Mule)
                {
                    account.Proxy.BannedMules++;
                }
                else
                {
                    account.Proxy.BannedBots++;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
