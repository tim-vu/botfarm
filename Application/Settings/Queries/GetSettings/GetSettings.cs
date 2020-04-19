using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FORFarm.Application.Common.Interfaces;
using MediatR;

namespace FORFarm.Application.Settings.Queries.GetSettings
{
    public class GetSettings : IRequest<SettingsVm>
    {
        
    }
    
    public class GetSettingsHandler : IRequestHandler<GetSettings, SettingsVm>
    {
        private readonly IMapper _mapper;
        private readonly IFarmContext _context;
        public GetSettingsHandler(IMapper mapper, IFarmContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task<SettingsVm> Handle(GetSettings request, CancellationToken cancellationToken)
        {
            return _context.FarmSettings.ContinueWith(r => _mapper.Map<SettingsVm>(r.Result));
        }
    }
}