using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccount, Unit>
    {
        private readonly IFarmContext _context;

        public DeleteAccountHandler(IFarmContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(DeleteAccount request, CancellationToken cancellationToken)
        {
            return _context.Accounts.Where(a => a.ID == request.ID).DeleteFromQueryAsync().ContinueWith(r => Unit.Value);
        }
    }
}