using MediatR;

namespace FORFarm.Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccount : IRequest
    {
        public int ID { get; set; }
    }
}