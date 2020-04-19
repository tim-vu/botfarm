using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Settings.Commands.UpdateSettings
{
    public class UpdateSettingsHandler : IRequestHandler<UpdateSettings>
    {
        private readonly IFarmContext _context;
        private readonly IMapper _mapper;

        public UpdateSettingsHandler(IFarmContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<Unit> Handle(UpdateSettings request, CancellationToken cancellationToken)
        {
            var settings = _mapper.Map<FarmSettings>(request.Settings);
            settings.ID = 1;

            _context.FarmSettingsTable.Update(settings);
            
            return _context.SaveChangesAsync(cancellationToken).ContinueWith(r => Unit.Value);
        }
    }
}