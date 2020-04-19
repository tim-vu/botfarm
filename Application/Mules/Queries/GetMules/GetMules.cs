using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FORFarm.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Mules.Queries.GetMules
{
    public class GetMules : IRequest<List<MuleVm>>
    {
        
    }
    
    public class GetMulesHandler : IRequestHandler<GetMules, List<MuleVm>>
    {
        private readonly IFarmContext _context;
        private readonly IMapper _mapper;
        
        public GetMulesHandler(IFarmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<MuleVm>> Handle(GetMules request, CancellationToken cancellationToken)
        {
            return _context.Mules
                .Include(m => m.Account)
                .ThenInclude(a => a.Proxy)
                .ProjectTo<MuleVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}