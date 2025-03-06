using BookstoreGraphQL.Application.DTOs;
using BookstoreGraphQL.Application.Features.Books.Queries;
using BookstoreGraphQL.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace BookstoreGraphQL.API.GraphQL.Queries
{
    public class BookQueries
    {
        [Authorize]
        public async Task<IEnumerable<BookDto>> GetBooks([Service] IMediator mediator)
            => await mediator.Send(new GetBooksQuery());

        public async Task<Book> GetBookById([Service] IMediator mediator, int id)
            => await mediator.Send(new GetBookByIdQuery { Id = id });
    }
}
