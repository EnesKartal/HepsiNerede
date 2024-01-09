using System.Threading.Tasks;

namespace HepsiNerede.Domain.Aggregates.Base
{
    /// <summary>
    /// Interface for base repository operations.
    /// </summary>
    public interface IBaseRepository
    {
        /// <summary>
        /// Begins a transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RollbackTransactionAsync();
    }
}
