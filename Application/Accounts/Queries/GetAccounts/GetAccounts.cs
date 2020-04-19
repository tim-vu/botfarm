using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Accounts.Queries.GetAccounts
{
    public class GetAccounts : IRequest<List<AccountVm>>
    {
    }
    
    public class GetAccountsHandler : IRequestHandler<GetAccounts, List<AccountVm>>
    {
        private readonly IFarmContext _context;
        private readonly IMapper _mapper;

        public GetAccountsHandler(IFarmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public Task<List<AccountVm>> Handle(GetAccounts request, CancellationToken cancellationToken)
        {
            return _context.Accounts
                .Include(a => a.Skills)
                .ToListAsync(cancellationToken)
                .ContinueWith(t => t.Result.Select(a => _mapper.Map<AccountVm>(a)).ToList());
        }
    }
}
