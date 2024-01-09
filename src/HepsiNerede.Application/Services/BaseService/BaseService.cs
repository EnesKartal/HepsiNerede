using HepsiNerede.Domain.Aggregates.Base;

namespace HepsiNerede.Application.Services.BaseService
{
    /// <summary>
    /// Interface defining basic service operations.
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// Initiates a new transaction.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits an initiated transaction, making database changes permanent.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back an initiated transaction, reverting database changes.
        /// </summary>
        Task RollbackTransactionAsync();
    }

    /// <summary>
    /// General service class implementing basic service operations.
    /// </summary>
    /// <typeparam name="T">Type of repository implementing the IBaseRepository interface.</typeparam>
    public class BaseService<T> : IBaseService where T : IBaseRepository
    {
        protected readonly T _repository;

        /// <summary>
        /// Constructor for the BaseService class.
        /// </summary>
        /// <param name="repository">Repository implementing the IBaseRepository interface.</param>
        public BaseService(T repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task BeginTransactionAsync()
        {
            await _repository.BeginTransactionAsync();
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync()
        {
            await _repository.CommitTransactionAsync();
        }

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync()
        {
            await _repository.RollbackTransactionAsync();
        }
    }
}
