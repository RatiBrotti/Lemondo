namespace Lemondo.DbClasses
{
    public class Book
    {
        public Book()
        {
            this.Authors = new List<Author>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? Rating { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int BooksQuantity { get; set; } 

        public List<Author> Authors { get; set; }
    }
}
