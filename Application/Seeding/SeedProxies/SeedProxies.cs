using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Seeding.SeedProxies
{
    public class SeedProxies : IRequest
    {
        
    }
    
    public class SeedProxiesHandler : IRequestHandler<SeedProxies>
    {
        private readonly IFarmContext _context;

        public SeedProxiesHandler(IFarmContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(SeedProxies request, CancellationToken cancellationToken)
        {
            _context.Proxies.AddRange(Proxies.Generate(50));
            return _context.SaveChangesAsync(cancellationToken).ContinueWith(r => Unit.Value);
        }
        
        private const int MinimumPort = 1;
        private const int MaximumPort = 65535;
        
        private static readonly Faker<Proxy> Proxies = new Faker<Proxy>().Rules((f, o) =>
        {
            o.Ip = f.Internet.Ip();
            o.Port = f.Random.Int(MinimumPort, MaximumPort);
            o.Username = f.Internet.UserName();
            o.Password = f.Internet.Password();
            o.BannedBots = f.Random.Number(0, 3);
        });
    }
}