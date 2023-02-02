using Lemondo.Context;
using Lemondo.UnitofWork.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Lemondo.UnitofWork.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected LibraryContext _context;

        internal DbSet<T> dbset;
        private LibraryContext context;
        private object logger;
        protected readonly ILogger _logger;

        public GenericRepository(ILogger logger, LibraryContext context)
        {
            _logger = logger;
            this.dbset = context.Set<T>();
            this.context = context;
        }

        public GenericRepository(LibraryContext context, object logger)
        {
            this.context = context;
            this.logger = logger;
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

        public virtual async Task<IQueryable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return (IQueryable<T>)await dbset.Where(predicate).ToListAsync();
        }

        public virtual async Task<IQueryable<T>> GetAll()
        {
            return dbset;
        }

        public virtual async Task<T> GetById(int id)
        {
            return await dbset.FindAsync(id);
        }

        public virtual async Task<bool> Upsert(T entity)
        {
            var dbentity = await dbset.FindAsync(entity);
            if (dbentity != null)
            {
                dbset.Update(entity);
                return true;
            }
            
            return false;
        }
    }
}
