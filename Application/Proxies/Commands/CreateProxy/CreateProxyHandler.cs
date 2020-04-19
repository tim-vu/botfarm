using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Proxies.Queries;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Proxies.Commands.CreateProxy
{
    public class CreateProxyHandler : IRequestHandler<CreateProxy, ProxyVm>
    {
        private readonly IFarmContext _context;
        private readonly IMapper _mapper;

        public CreateProxyHandler(IFarmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProxyVm> Handle(CreateProxy request, CancellationToken cancellationToken)
        {
            var proxy = new Proxy
            {
                Ip = request.Ip,
                Port = request.Port,
                Username = request.Username,
                Password = request.Password
            };

            _context.Proxies.Add(proxy);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProxyVm>(proxy);
        }
    }
}
