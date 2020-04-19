using FORFarm.Application.Accounts.Queries;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccount : IRequest<AccountVm>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool Mule { get; set; }

        public int RemainingMembershipDays { get; set; }

    }
}
