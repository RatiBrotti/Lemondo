using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



namespace Lemondo.UnitofWork.Repository
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryContext context, ILogger logger) : base(context, logger)
        {

        }
        public override async Task<IQueryable<Book>> GetAll()
        {
            try
            {
                return dbset;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(BookRepository));
                return Enumerable.Empty<Book>().AsQueryable();
            }
        }
        public override async Task<bool> Upsert(Book entity)
        {
            try
            {
                var existingEntity = await dbset.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                    return await Add(entity);

                existingEntity.Title = entity.Title;
                existingEntity.Description = entity.Description;
                existingEntity.Rating = entity.Rating;
                existingEntity.Image = entity.Image;
                existingEntity.PublicationDate = entity.PublicationDate;
                existingEntity.IsCheckedOut = entity.IsCheckedOut;


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(BookRepository));
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
                _logger.LogError(ex, "{Repo} Delete function error", typeof(BookRepository));
                return false;
            }
        }
    }
}
