using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;

namespace Lemondo.UnitofWork.Repository
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryContext context, ILogger<Author> logger) : base(logger, context)
        {

        }
    }
}

