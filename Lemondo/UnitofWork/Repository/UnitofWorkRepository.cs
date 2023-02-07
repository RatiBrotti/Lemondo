using Lemondo.UnitofWork.Interface;
using Lemondo.Context;
using Lemondo.DbClasses;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Lemondo.UnitofWork.Repository
{
    public class UnitofWorkRepository : IUnitofWork, IDisposable
    {
        private readonly LibraryContext _context;
        private readonly ILogger _logger;

        public IUserRepository User { get; private set; }
        public IBookRatingRepository BookRating { get; private set; }
        public IBookAuthorRepository BookAuthor { get; private set; }
        public IAuthorRepository Author { get; private set; }
        public IBookRepository Book { get; private set; }

        public UnitofWorkRepository(LibraryContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Book = new BookRepository(context, _logger);
            Author = new AuthorRepository(context, _logger);
            BookAuthor = new BookAuthorRepository(context, _logger);
            BookRating = new BookRatingRepository(context, _logger);
            User = new UserRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
