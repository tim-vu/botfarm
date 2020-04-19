using Application.UnitTests.Common;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Farm;
using FORFarm.Domain.Entities;

namespace Application.UnitTests.Farm.FarmBuilder
{
    public class FarmBuilderTestBase : TestBase
    {
        protected const int MaxActiveBots = 9; 
        protected const int MaxActiveMules = 3;
        protected const int MinActiveMules = 1;
        protected const int ConcurrentAccountsPerProxy = 3;
        protected const int MaxAccountsPerProxy = 6;

        public FarmBuilderTestBase()
        {
            var settings = Clone(Data.Settings);
            settings.ID = 1;

            var context = NewContext;
            context.Update(settings);

            context.SaveChanges();
        }

        protected static readonly FarmSettings Settings = new FarmSettings
        {
            UseProxies = true,
            MaxActiveBots = MaxActiveBots,
            MinActiveMules = MinActiveMules,
            MaxActiveMules = MaxActiveMules
        };
    }
}