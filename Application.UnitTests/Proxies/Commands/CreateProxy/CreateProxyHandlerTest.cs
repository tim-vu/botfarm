using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FluentAssertions.Common;
using FORFarm.Application.Proxies.Commands.CreateProxy;
using FORFarm.Application.Proxies.Queries;
using Xunit;

namespace Application.UnitTests.Proxies.Commands.CreateProxy
{
    public class CreateProxyHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_CreateProxy_NewIp()
        {
            var proxy = Data.Proxies.First();

            var createProxyHandler = new CreateProxyHandler(NewContext, Mapper);

            await createProxyHandler.Handle(
                new FORFarm.Application.Proxies.Commands.CreateProxy.CreateProxy()
                {
                    Ip = proxy.Ip, Port = proxy.Port, Username = proxy.Username, Password = proxy.Password
                }, CancellationToken.None);
            
            NewContext.Proxies.ToList().Should().ContainSingle(p =>
                p.Ip == proxy.Ip && 
                p.Port == proxy.Port && 
                p.Username == proxy.Username &&
                p.Password == proxy.Password);
        }
    }
}
