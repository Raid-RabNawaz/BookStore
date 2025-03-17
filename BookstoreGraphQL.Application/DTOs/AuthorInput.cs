
using HotChocolate;

namespace BookstoreGraphQL.Application.DTOs
{
    public record AuthorInput(
        [GraphQLNonNullType] string Name
    );
}
