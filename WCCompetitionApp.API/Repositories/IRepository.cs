using System.Linq.Expressions;

namespace WCCompetitionApp.API.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(object id);
        Task<T> GetById(object id);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null);
    }
}
