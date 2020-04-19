using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ForFarmDbContextFactory : DesignTimeDbContextFactoryBase<ForFarmDbContext>
    {
        protected override ForFarmDbContext CreateNewInstance(DbContextOptions<ForFarmDbContext> options)
        {
            return new ForFarmDbContext(options);
        }
    }
}