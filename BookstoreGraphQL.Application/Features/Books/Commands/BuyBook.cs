using BookstoreGraphQL.Domain.Interfaces;
using MediatR;

namespace BookstoreGraphQL.Application.Features.Books.Commands
{
    public class BuyBookCommand : IRequest<bool>
    {
        public int BookId { get; set; }
    }

    public class BuyBookCommandHandler : IRequestHandler<BuyBookCommand, bool>
    {
        private readonly IBookRepository _repository;

        public BuyBookCommandHandler(IBookRepository repository) => _repository = repository;

        public async Task<bool> Handle(BuyBookCommand request, CancellationToken cancellationToken)
        {
            return await _repository.BuyBookAsync(request.BookId);
        }
    }
}
