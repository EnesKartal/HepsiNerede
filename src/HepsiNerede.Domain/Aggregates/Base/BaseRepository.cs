namespace HepsiNerede.Domain.Aggregates.Base
{
    public interface IBaseRepository
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
