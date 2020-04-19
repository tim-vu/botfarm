using System.Collections.Generic;
using System.Threading.Tasks;
using FORFarm.Application.Bots.Queries;
using FORFarm.Application.Bots.Queries.GetBots;
using FORFarm.Application.Mules.Queries;
using FORFarm.Application.Mules.Queries.GetMules;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class InstancesController : BaseController
    {
        public InstancesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("bots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BotVm>>> GetAllBots()
        {
            return Ok(await Mediator.Send(new GetBots()));
        }

        [HttpGet("mules")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MuleVm>>> GetAllMules()
        {
            return Ok(await Mediator.Send(new GetMules()));
        }
    }
}