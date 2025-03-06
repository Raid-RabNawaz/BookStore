
namespace BookstoreGraphQL.Application.DTOs
{
    public record BookDto(int Id, string Title, int AuthorId, string AuthorName, int Stock, decimal Price);
}
