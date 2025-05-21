
using Domain.Queries;

namespace Domain.Infrastructure
{
    public interface IQueryDispatcher<TEntity>
    {
        void RegisterHandlers<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : BaseQuery;
        Task<List<TEntity>> SendAsync(BaseQuery query);
    }
}
