namespace Lemondo.DbClasses
{
    public class BookRating
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int? Rating { get; set; }

        public Book Book { get; set; }

    }
}
