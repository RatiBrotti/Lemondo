namespace Lemondo.DbClasses
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
