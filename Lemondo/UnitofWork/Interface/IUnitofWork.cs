using Lemondo.DbClasses;

namespace Lemondo.UnitofWork.Interface
{
    public interface IUnitofWork : IGenericRepository<User>
    {
        IUserRepository User { get; }
        IBookRatingRepository BookRating { get; }
        IBookAuthorRepository BookAuthor { get; }
        IAuthorRepository Author { get; }
        IBookRepository Book { get; }
        Task CompleteAsync();
        void Dispose();
    }
}
