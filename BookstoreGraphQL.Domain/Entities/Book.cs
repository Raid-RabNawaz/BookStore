
namespace BookstoreGraphQL.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public Author Author { get; set; }
    }
}
