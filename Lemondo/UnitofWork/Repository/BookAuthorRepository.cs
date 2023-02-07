using Lemondo.Context;
using Lemondo.DbClasses;
using Lemondo.UnitofWork.Interface;


namespace Lemondo.UnitofWork.Repository
{
    public class BookAuthorRepository : GenericRepository<BookAuthor>, IBookAuthorRepository
    {

        public BookAuthorRepository(LibraryContext context, ILogger logger) : base(logger, context)
        {

        }
        
    }
}
