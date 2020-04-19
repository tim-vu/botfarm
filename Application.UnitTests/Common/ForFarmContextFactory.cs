using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using FORFarm.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitTests.Common
{
    public class ForFarmContextFactory
    {
        public static ForFarmDbContext Create(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<ForFarmDbContext>()
                .UseSqlite(connection)
                .Options;
            //.UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new ForFarmDbContext(options);
        }

        public static void Destroy(ForFarmDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
