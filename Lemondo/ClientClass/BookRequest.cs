namespace Lemondo.ClientClass
{
    public class BookRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? Rating { get; set; }    
        public List<int>? AuthorId { get; set; }
        public DateTime? PublicationDate { get; set; }
        public bool? IsCheckedOut { get; set; }
    }
}
