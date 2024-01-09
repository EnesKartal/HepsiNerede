using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Infrastructure.DbContexts;

namespace HepsiNerede.Infrastructure.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly HepsiNeredeDBContext _dbContext;
        public BaseRepository(HepsiNeredeDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }
        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
