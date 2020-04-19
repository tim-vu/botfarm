using System.Collections.Generic;
using System.Threading.Tasks;
using FORFarm.Application.Bots.Queries;
using FORFarm.Application.Bots.Queries.GetBots;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class BotsController : BaseController
    {
        public BotsController(IMediator mediator) : base(mediator)
        {
        }
        
    }
}