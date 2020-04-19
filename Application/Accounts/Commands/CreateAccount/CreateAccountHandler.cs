using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FORFarm.Application.Accounts.Queries;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using FORFarm.Domain.Enums;
using FORFarm.Domain.ValueObjects;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccount, AccountVm>
    {
        private readonly IFarmContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public CreateAccountHandler(IFarmContext context, IDateTime dateTime, IMapper mapper)
        {
            _context = context;
            _dateTime = dateTime;
            _mapper = mapper;
        }

        public async Task<AccountVm> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Username = request.Username,
                Password = request.Password,
                Mule = request.Mule,
                MemberExpirationDate = _dateTime.UtcNow.AddDays(request.RemainingMembershipDays)
            };

            foreach (var skill in Skills.DefaultSkills)
            {
                account.Skills.Add(skill);
            }
            _context.Accounts.Add(account);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AccountVm>(account);
        }
    }
}
