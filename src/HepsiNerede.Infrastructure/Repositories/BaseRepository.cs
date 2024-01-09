using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Infrastructure.DbContexts;

namespace HepsiNerede.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the base repository providing common database transaction functionality.
    /// </summary>
    public class BaseRepository : IBaseRepository
    {
        /// <summary>
        /// The database context used by the repository.
        /// </summary>
        protected readonly HepsiNeredeDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to be used by the repository.</param>
        public BaseRepository(HepsiNeredeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits the current database transaction asynchronously.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        /// <summary>
        /// Rolls back the current database transaction asynchronously.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
