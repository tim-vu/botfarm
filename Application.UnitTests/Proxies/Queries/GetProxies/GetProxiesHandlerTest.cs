using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.UnitTests.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentAssertions;
using FORFarm.Application.Proxies.Queries;
using FORFarm.Application.Proxies.Queries.GetProxies;
using FORFarm.Domain.Entities;
using MediatR;
using Moq;
using Persistence;
using Xunit;

namespace Application.UnitTests.Proxies.Queries.GetProxies
{

    public class GetProxiesHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_GetProxies()
        {
            var proxies = Data.Proxies;

            using (var context = NewContext)
            {
                context.Proxies.AddRange(proxies);
                await context.SaveChangesAsync();
            }

            var getProxiesHandler = new GetProxiesHandler(NewContext, Mapper);

            var result =
                await getProxiesHandler.Handle(new FORFarm.Application.Proxies.Queries.GetProxies.GetProxies(),
                    CancellationToken.None);

            result.Should().BeEquivalentTo(proxies.Select(p => Mapper.Map<ProxyVm>(p)));
        }
    }
}
