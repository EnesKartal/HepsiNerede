using HepsiNerede.Domain.Aggregates.Base;

namespace HepsiNerede.Application.Services.BaseService
{
    public interface IBaseService
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

    public class BaseService<T> : IBaseService where T : IBaseRepository
    {
        protected readonly T _repository;
        public BaseService(T repository)
        {
            _repository = repository;
        }

        public async Task BeginTransactionAsync()
        {
            await _repository.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _repository.CommitTransactionAsync();
        }
        public async Task RollbackTransactionAsync()
        {
            await _repository.RollbackTransactionAsync();
        }
    }
}
