using BookstoreGraphQL.Application.DTOs;
using BookstoreGraphQL.Domain.Interfaces;
using MediatR;

namespace BookstoreGraphQL.Application.Features.Books.Queries
{
    public class GetBooksQuery : IRequest<IEnumerable<BookDto>> { }

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookRepository _repository;

        public GetBooksQueryHandler(IBookRepository repository) => _repository = repository;

        public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _repository.GetBooksAsync();
            return books.Select(b => new BookDto(b.Id, b.Title, b.AuthorId, b.Author.Name, b.Stock, b.Price));
        }
    }
}
