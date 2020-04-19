using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FORFarm.Application.Proxies.Commands.CreateProxy;
using FORFarm.Application.Proxies.Commands.DeleteProxy;
using FORFarm.Application.Proxies.Queries;
using FORFarm.Application.Proxies.Queries.GetProxies;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    
    public class ProxiesController : BaseController
    {
        public ProxiesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProxyVm>>> GetAll()
        {
            var result = await Mediator.Send(new GetProxies());
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProxyVm>> Create([FromBody] CreateProxy createProxy)
        {
            var proxy = await Mediator.Send(createProxy);
            return CreatedAtAction(nameof(GetAll), proxy);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteProxy(id));
            return NoContent();
        }
    }
    
}