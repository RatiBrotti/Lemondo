using Lemondo.UnitofWork.Interface;
using Lemondo.Context;
using Lemondo.DbClasses;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Lemondo.UnitofWork.Repository
{
    public class UnitofWorkRepository : IUnitofWork , IDisposable
    {
        private readonly LibraryContext _context;

        public IUserRepository User { get; private set; }
        public IAuthorRepository Author { get; private set; }
        public IBookRepository Book { get; private set; }

        public UnitofWorkRepository(IUserRepository user, IAuthorRepository author, IBookRepository book, LibraryContext context)
        {
            User = user;
            Author = author;
            Book = book;
            _context = context;
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
