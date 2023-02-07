using Castle.Core.Internal;

namespace Lemondo.DbClasses
{
    public class Author
    {
        public Author()
        {
            this.Books = new List<Book>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? YearOfBirth { get; set; }

        public List<Book> Books { get; set; }
    }
}
