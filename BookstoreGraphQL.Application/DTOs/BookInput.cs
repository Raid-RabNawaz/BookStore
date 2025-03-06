using HotChocolate;

namespace BookstoreGraphQL.Application.DTOs
{
    public record BookInput(
        [GraphQLNonNullType] string Title,
        [GraphQLNonNullType] int AuthorId,
        [GraphQLNonNullType] int Stock,
        [GraphQLNonNullType] decimal Price
    );
}
