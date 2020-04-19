using System.Threading.Tasks;
using FORFarm.Application.Status;
using FORFarm.Application.Status.GetStatus;
using FORFarm.Application.Status.UpdateStatus;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class StatusController : BaseController
    {
        public StatusController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StatusVm>> Get()
        {
            return Ok(await Mediator.Send(new GetStatus()));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StatusVm>> Update([FromBody] UpdateStatus updateStatus)
        {
            return Ok(await Mediator.Send(updateStatus));
        }
    }
}