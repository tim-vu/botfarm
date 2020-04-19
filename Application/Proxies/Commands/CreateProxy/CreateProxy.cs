using System;
using System.Collections.Generic;
using System.Text;
using FORFarm.Application.Proxies.Queries;
using FORFarm.Domain.Entities;
using MediatR;

namespace FORFarm.Application.Proxies.Commands.CreateProxy
{
    public class CreateProxy : IRequest<ProxyVm>
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
