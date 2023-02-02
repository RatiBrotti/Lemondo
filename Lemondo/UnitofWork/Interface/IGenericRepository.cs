using System.Linq.Expressions;

namespace Lemondo.UnitofWork.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        Task<bool> Upsert(T entity);
        Task<IQueryable<T>>Find(Expression<Func<T, bool>> predicate);

    }
}
