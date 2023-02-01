namespace Lemondo.DbClasses
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? Rating { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsCheckedOut { get; set; }

        public List<BookRating> BookRatings { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
