using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Lemondo.UnitofWork.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected LibraryContext _context;

        internal DbSet<T> dbset;
        protected readonly ILogger _logger;

        public GenericRepository(ILogger logger, LibraryContext context)
        {
            _logger = logger;
            this._context = context;
            dbset = _context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbset.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await dbset.FindAsync(id);
            if (entity != null)
            {
                dbset.Remove(entity);
                return true;
            }
            return false;

        }

        public virtual async Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate)
        {
            var authors = await dbset.Where(predicate).ToListAsync();
            return authors.AsQueryable();
        }

        public virtual async Task<DbSet<T>> All()
        {
            return dbset;
        }

        public virtual async Task<List<T>> GetAllToList()
        {
            return dbset.ToList();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await dbset.FindAsync(id);
        }
    }
}
