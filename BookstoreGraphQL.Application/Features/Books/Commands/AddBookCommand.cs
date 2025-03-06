using BookstoreGraphQL.Application.DTOs;
using BookstoreGraphQL.Domain.Entities;
using BookstoreGraphQL.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace BookstoreGraphQL.Application.Features.Books.Commands
{
    public class AddBookCommand : IRequest<BookDto>
    {
        public BookInput Input { get; set; }
    }

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookDto>
    {
        private readonly IBookRepository _repository;

        private readonly IValidator<BookInput> _validator;

        public AddBookCommandHandler(IBookRepository repository, IValidator<BookInput> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<BookDto> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.Input);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var book = new Book
            {
                Title = request.Input.Title,
                AuthorId = request.Input.AuthorId,
                Stock = request.Input.Stock,
                Price = request.Input.Price
            };

            await _repository.AddBookAsync(book);
            return new BookDto(book.Id, book.Title, book.AuthorId, "Unknown", book.Stock, book.Price);
        }
    }
}
