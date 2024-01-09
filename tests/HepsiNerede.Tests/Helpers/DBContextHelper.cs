using HepsiNerede.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Tests.Helpers
{
    public static class DBContextHelper
    {
        public static HepsiNeredeDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<HepsiNeredeDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            var dbContext = new HepsiNeredeDbContext(options);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
