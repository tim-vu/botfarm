using System;
using MediatR;

namespace FORFarm.Application.Bots.Commands.RequestMuling
{
    public class RequestMuling : IRequest<RequestMulingResponse>
    {
        public Guid Tag { get; set; }
    }
}