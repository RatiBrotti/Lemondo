using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using System.Data.Entity;


namespace Lemondo.UnitofWork.Repository
{
    public class BookAuthorRepository : GenericRepository<BookAuthor>, IBookAuthorRepository
    {

        public BookAuthorRepository(LibraryContext context, ILogger logger) : base(context, logger)
        {

        }
        public override async Task<IQueryable<BookAuthor>> GetAll()
        {
            try
            {
                return dbset;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(BookAuthorRepository));
                return Enumerable.Empty<BookAuthor>().AsQueryable();
            }
        }
        public override async Task<bool> Upsert(BookAuthor entity)
        {
            try
            {
                var existingEntity = await dbset.Where(x => x.BookId == entity.BookId)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                    return await Add(entity);

                existingEntity.BookId = entity.BookId;
                existingEntity.AuthorId = entity.AuthorId;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(BookratingRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbset.Where(x => x.BookId == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbset.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(BookratingRepository));
                return false;
            }
        }
    }
}
