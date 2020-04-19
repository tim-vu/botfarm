using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Bots.Commands.FinishMuling
{
    public class FinishMulingHandler : IRequestHandler<FinishMuling>
    {
        private readonly IFarmContext _context;

        public FinishMulingHandler(IFarmContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(FinishMuling request, CancellationToken cancellationToken)
        {
            var muleRequest = await _context.MuleRequests
                .Include(m => m.Bot)
                .Where(m => m.Bot.Tag == request.Tag)
                .FirstOrDefaultAsync(cancellationToken);

            if (muleRequest == null)
            {
                throw new NotFoundException("MuleRequest", "Tag", request.Tag.ToString());
            }

            var bot = muleRequest.Bot;
            bot.GoldEarned += request.GoldTransferred;

            _context.MuleRequests.Remove(muleRequest);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}