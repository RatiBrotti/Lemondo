using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Lemondo.UnitofWork.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<DbSet<T>> All();
        Task<List<T>> GetAllToList();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate);

    }
}
