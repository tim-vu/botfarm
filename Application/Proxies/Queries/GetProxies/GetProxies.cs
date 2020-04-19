
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FORFarm.Application.Common.Interfaces;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Proxies.Queries.GetProxies
{
    public class GetProxies : IRequest<List<ProxyVm>>
    {

    }

    public class GetProxiesHandler : IRequestHandler<GetProxies, List<ProxyVm>>
    {
        private readonly IFarmContext _context;
        private readonly IMapper _mapper;

        public GetProxiesHandler(IFarmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public Task<List<ProxyVm>> Handle(GetProxies request, CancellationToken cancellationToken)
        {
            return _context.Proxies.ProjectTo<ProxyVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}
