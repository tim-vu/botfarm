using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;
        
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}