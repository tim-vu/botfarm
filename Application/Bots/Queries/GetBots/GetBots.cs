using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FORFarm.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Bots.Queries.GetBots
{
    public class GetBots : IRequest<List<BotVm>>
    {
        
    }
    
    public class GetBotHandler : IRequestHandler<GetBots, List<BotVm>>
    {
        private readonly IFarmContext _context;
        private readonly IMapper _mapper;

        public GetBotHandler(IFarmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<BotVm>> Handle(GetBots request, CancellationToken cancellationToken)
        {
            return _context.Bots
                .Include(b =>  b.Account)
                .ThenInclude(a => a.Proxy)
                .ProjectTo<BotVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}