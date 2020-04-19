using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Accounts.Queries;
using Xunit;

namespace Application.UnitTests.Accounts.Queries.GetAccounts
{
    public class GetAccountsHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_GetAccounts()
        {
            var accounts = BogusData.MixedAccounts.Generate(5);
            accounts.ForEach(a => a.Proxy = BogusData.Proxies.Generate());

            await using (var context = NewContext)
            {
                context.Accounts.AddRange(accounts);
                await context.SaveChangesAsync();
            }

            var getAccountsHandler = new FORFarm.Application.Accounts.Queries.GetAccounts.GetAccountsHandler(NewContext, Mapper);
            var result = await getAccountsHandler.Handle(new FORFarm.Application.Accounts.Queries.GetAccounts.GetAccounts(),
                CancellationToken.None);

            result.Should().BeEquivalentTo(accounts.Select(a => Mapper.Map<AccountVm>(a)));
        }
    }
}
