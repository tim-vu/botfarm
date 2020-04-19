using FORFarm.Application.Accounts.Queries;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccount : IRequest
    {
        public int ID { get; set; }
        
        public string Password { get; set; }

        public bool Mule { get; set; }
    }
}
