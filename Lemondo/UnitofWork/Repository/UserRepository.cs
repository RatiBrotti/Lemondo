using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;
using System.Data.Entity;

namespace Lemondo.UnitofWork.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LibraryContext context, ILogger logger) : base(context, logger)
        {

        }
        public override async Task<IQueryable<User>> GetAll()
        {
            try
            {
                return dbset;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(UserRepository));
                return Enumerable.Empty<User>().AsQueryable();
            }
        }
        public override async Task<bool> Upsert(User entity)
        {
            try
            {
                var existingEntity = await dbset.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingEntity == null)
                    return await Add(entity);

                existingEntity.FirstName = entity.FirstName;
                existingEntity.LastName = entity.LastName;
                existingEntity.Email = entity.Email;


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(UserRepository));
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
                _logger.LogError(ex, "{Repo} Delete function error", typeof(UserRepository));
                return false;
            }
        }
    }
}
