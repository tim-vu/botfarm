using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Accounts.Queries;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using FORFarm.Domain.ValueObjects;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.UpdateGameDetails
{
    public class UpdateGameDetailsHandler : IRequestHandler<UpdateGameDetails>
    {
        private readonly IFarmContext _context;
        private readonly IDateTime _dateTime;

        public UpdateGameDetailsHandler(IFarmContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(UpdateGameDetails request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FindAsync(request.ID);

            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.ID);
            }

            account.MemberExpirationDate = _dateTime.UtcNow.AddDays(request.MembershipDaysRemaining);

            if (request.Skills != null)
            {
                account.Skills = request.Skills.Keys.Select(k => new Skill(k, request.Skills[k])).ToList();
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
