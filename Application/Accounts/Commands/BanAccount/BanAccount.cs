using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace FORFarm.Application.Accounts.Commands.BanAccount
{
    public class BanAccount : IRequest
    {
        public int ID { get; set; }
    }

}
