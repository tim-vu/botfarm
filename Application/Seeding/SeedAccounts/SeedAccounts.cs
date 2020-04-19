using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using FORFarm.Domain.Enums;
using FORFarm.Domain.ValueObjects;
using MediatR;

namespace FORFarm.Application.Seeding.SeedAccounts
{
    public class SeedAccounts : IRequest
    {
        public int Amount { get; set; } = 50;
    }
    
    public class SeedAccountsHandler : IRequestHandler<SeedAccounts>
    {
        private readonly IFarmContext _context;

        public SeedAccountsHandler(IFarmContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(SeedAccounts request, CancellationToken cancellationToken)
        {
           _context.Accounts.AddRange(MixedAccounts.Generate(request.Amount));
           return _context.SaveChangesAsync(cancellationToken).ContinueWith(r => Unit.Value);
        }
        
        public static readonly Faker<Account> MixedAccounts = new Faker<Account>().Rules((f, o) =>
        {
            o.Username = f.Internet.UserName();
            o.Password = f.Internet.Password();
            o.Runtime = f.Date.Timespan(TimeSpan.FromDays(4));
            o.Mule = f.Random.Bool(0.2f);
            o.Banned = f.Random.Bool(0.1f);
            o.MemberExpirationDate = DateTime.UtcNow.AddDays(5);
            o.Skills = Skills.CreateNewDefaultSkills();
            o.GoldEarned = f.Random.Number(1000 * 000, 23000000);
        });
    }
}