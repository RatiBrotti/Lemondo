using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using System.Data.Entity;

namespace Lemondo.UnitofWork.Repository
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryContext context, ILogger logger) : base(context, logger)
        {

        }
        public override async Task<IQueryable<Author>> GetAll()
        {
            try
            {
                return dbset;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(AuthorRepository));
                return Enumerable.Empty<Author>().AsQueryable();
            }
        }
        public override async Task<bool> Upsert(Author entity)
        {
            try
            {
                var existingEntity = await dbset.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                    return await Add(entity);

                existingEntity.FirstName = entity.FirstName;
                existingEntity.LastName = entity.LastName;
                existingEntity.YearOfBirth = entity.YearOfBirth;


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(AuthorRepository));
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
                _logger.LogError(ex, "{Repo} Delete function error", typeof(AuthorRepository));
                return false;
            }
        }
    }
}

