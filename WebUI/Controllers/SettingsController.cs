using System.Threading.Tasks;
using FORFarm.Application.Settings;
using FORFarm.Application.Settings.Commands.UpdateSettings;
using FORFarm.Application.Settings.Queries.GetSettings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    
    public class SettingsController : BaseController
    {
        public SettingsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SettingsVm>> Get()
        {
            var result = await Mediator.Send(new GetSettings());
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromBody] SettingsVm settings)
        {
            await Mediator.Send(new UpdateSettings(settings));
            return NoContent();
        }
    }
}