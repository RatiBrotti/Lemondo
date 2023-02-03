using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using System.Data.Entity;

namespace Lemondo.UnitofWork.Repository
{
    public class BookRatingRepository : GenericRepository<BookRating>, IBookRatingRepository
    {
        public BookRatingRepository(LibraryContext context, ILogger logger) : base(context, logger)
        {

        }
        public override async Task<IQueryable<BookRating>> GetAll()
        {
            try
            {
                return dbset;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(BookRatingRepository));
                return Enumerable.Empty<BookRating>().AsQueryable();
            }
        }
        public override async Task<bool> Upsert(BookRating entity)
        {
            try
            {
                var existingEntity = await dbset.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                    return await Add(entity);

                existingEntity.BookId = entity.BookId;
                existingEntity.UserId = entity.UserId;
                existingEntity.Rating = entity.Rating;


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(BookRating));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbset.Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbset.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(BookRatingRepository));
                return false;
            }
        }
    }
}
