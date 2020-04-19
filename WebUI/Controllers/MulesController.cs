using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FORFarm.Application.Mules.Queries;
using FORFarm.Application.Mules.Queries.GetMules;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class MulesController : BaseController
    {
        public MulesController(IMediator mediator) : base(mediator)
        {
        }
        
    }
}