using System;
using MediatR;

namespace FORFarm.Application.Bots.Commands.FinishMuling
{
    public class FinishMuling : IRequest
    {
        public Guid Tag { get; set; }
        
        public int GoldTransferred { get; set; }
    }
}