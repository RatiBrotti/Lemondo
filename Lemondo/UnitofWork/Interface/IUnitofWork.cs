using Lemondo.DbClasses;

namespace Lemondo.UnitofWork.Interface
{
    public interface IUnitofWork
    {
        IUserRepository User { get; }
        //IBookRatingRepository BookRating { get; }
        //IBookAuthorRepository BookAuthor { get; }
        IAuthorRepository Author { get; }
        IBookRepository Book { get; }
        Task CompleteAsync();
    }
}
