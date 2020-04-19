using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using MediatR;

namespace FORFarm.Application.Proxies.Commands.DeleteProxy
{
    public class DeleteProxy : IRequest
    {
        public int ID { get; }

        public DeleteProxy(int id)
        {
            ID = id;
        }
    }

    public class DeleteProxyHandler : IRequestHandler<DeleteProxy>
    {
        private readonly IFarmContext _context;

        public DeleteProxyHandler(IFarmContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(DeleteProxy request, CancellationToken cancellationToken)
        {
            /*
            var tableName = _context.Model.FindEntityType(typeof(Proxy)).GetTableName();

            var query = $"DELETE FROM {tableName} WHERE {tableName}.id = {{0}}";

            return _context.Database.ExecuteSqlRawAsync(query, request.Id).ContinueWith(r => Unit.Value);
            */
            
            //TODO: disallow deleting of proxy with active accounts while the farm is running

            return _context.Proxies
                .Where(p => p.ID == request.ID)
                .DeleteFromQueryAsync(cancellationToken)
                .ContinueWith(r => Unit.Value);
        }
    }
}
