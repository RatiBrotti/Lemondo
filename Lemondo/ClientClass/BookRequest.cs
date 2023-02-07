using Lemondo.DbClasses;

namespace Lemondo.ClientClass
{
    public class BookRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? Rating { get; set; }    
        public List<int> AuthorIds { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}
