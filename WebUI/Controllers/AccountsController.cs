using System.Collections.Generic;
using System.Threading.Tasks;
using FORFarm.Application.Accounts.Commands.BanAccount;
using FORFarm.Application.Accounts.Commands.CreateAccount;
using FORFarm.Application.Accounts.Commands.DeleteAccount;
using FORFarm.Application.Accounts.Commands.UpdateAccount;
using FORFarm.Application.Accounts.Commands.UpdateGameDetails;
using FORFarm.Application.Accounts.Queries;
using FORFarm.Application.Accounts.Queries.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AccountsController : BaseController
    {
        public AccountsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AccountVm>>> GetAll()
        {
            var result = await Mediator.Send(new GetAccounts());
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<AccountVm>> Create([FromBody] CreateAccount createAccount)
        {
            var account = await Mediator.Send(createAccount);
            return CreatedAtAction(nameof(GetAll), account);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateAccount updateAccount)
        {
            await Mediator.Send(updateAccount);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteAccount {ID = id});
            return NoContent();
        }
    }
}