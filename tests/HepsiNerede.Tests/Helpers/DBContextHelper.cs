using HepsiNerede.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Tests.Helpers
{
    public static class DBContextHelper
    {
        public static HepsiNeredeDBContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<HepsiNeredeDBContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            var dbContext = new HepsiNeredeDBContext(options);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
