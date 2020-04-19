using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FORFarm.Application.Common.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Z.EntityFramework.Plus;

namespace Application.UnitTests.Common
{
    public class TestBase : IDisposable
    {
        protected ForFarmDbContext NewContext => ForFarmContextFactory.Create(Connection);
        protected IMapper Mapper { get; }
        
        private SqliteConnection Connection { get; set; }

        public TestBase()
        { 
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();

            using (var context = NewContext)
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            ForFarmContextFactory.Destroy(NewContext);
            Connection.Close();
        }

        public static T Clone<T>(T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }
    }
}
