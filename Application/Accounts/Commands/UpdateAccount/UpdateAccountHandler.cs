using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FORFarm.Application.Accounts.Queries;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccount>
    {
        private readonly IFarmContext _context;

        public UpdateAccountHandler(IFarmContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(UpdateAccount request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                ID = request.ID,
                Password = request.Password, 
                Mule = request.Mule
            };

            var entry = _context.Accounts.Attach(account);
            entry.Property(a => a.Password).IsModified = true;
            entry.Property(a => a.Mule).IsModified = true;

            return _context.SaveChangesAsync(cancellationToken).ContinueWith(r => Unit.Value);
        }
    }
}
