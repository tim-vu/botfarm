using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Application.Proxies.Commands.DeleteProxy;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Z.EntityFramework.Extensions;

namespace Application.UnitTests.Proxies.Commands.DeleteProxy
{
    public class DeleteProxyHandlerTest : TestBase
    {
        public DeleteProxyHandlerTest()
        {
        }

        [Fact]
        public async void Handle_DeleteProxy()
        {
            //setup
            var proxy = Data.Proxies.First();
            var accounts = BogusData.CreateFaker(false, true, false).Generate(3);

            accounts.ForEach(a => proxy.ActiveAccounts.Add(a));

            using (var context = NewContext)
            {
                context.Proxies.Add(proxy);
                await context.SaveChangesAsync();
            }

            //act
            using (var context = NewContext)
            {
                var deleteProxyHandler = new DeleteProxyHandler(context);
                
                await deleteProxyHandler.Handle(new FORFarm.Application.Proxies.Commands.DeleteProxy.DeleteProxy(proxy.ID),
                    CancellationToken.None);
            }

            //verify
            using (var context = NewContext)
            {
                context.Proxies.ToList().Should().NotContain(p => p.ID == proxy.ID);

                foreach (var account in context.Accounts.Include(a => a.Proxy).ToList())
                {
                    account.Proxy.Should().BeNull();
                }
            }
        }
    }
}
