using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;

namespace Lemondo.UnitofWork.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LibraryContext context, ILogger logger) : base(logger, context)
        {

        }
    }
}
