using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Lemondo.UnitofWork.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<DbSet<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<T>>Find(Expression<Func<T, bool>> predicate);

    }
}
