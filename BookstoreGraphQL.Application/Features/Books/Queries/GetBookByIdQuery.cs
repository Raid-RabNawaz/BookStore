using BookstoreGraphQL.Domain.Entities;
using BookstoreGraphQL.Domain.Interfaces;
using MediatR;

namespace BookstoreGraphQL.Application.Features.Books.Queries
{
    public class GetBookByIdQuery : IRequest<Book>
    {
        public int Id { get; set; }
    }

    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly IBookRepository _repository;

        public GetBookByIdQueryHandler(IBookRepository repository) => _repository = repository;

        public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetBookByIdAsync(request.Id);
        }
    }
}
