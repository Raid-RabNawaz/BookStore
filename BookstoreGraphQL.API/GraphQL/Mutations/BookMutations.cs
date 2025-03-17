using BookstoreGraphQL.Application.DTOs;
using BookstoreGraphQL.Application.Features.Books.Commands;
using BookstoreGraphQL.Domain.Interfaces;
using MediatR;

namespace BookstoreGraphQL.API.GraphQL.Mutations
{
    public class BookMutations
    {
        public async Task<string> RegisterUser([Service] IAuthService authService, string fullName, string email, string password, string role)
        {
            return await authService.RegisterUser(fullName, email, password, role);
        }

        public async Task<string> LoginUser([Service] IAuthService authService, string email, string password)
        {
            return await authService.LoginUser(email, password);
        }

        public async Task<bool> LogoutUser([Service] IAuthService authService, string token)
        {
            return true;
        }
        public async Task<BookDto> AddBook([Service] IMediator mediator, BookInput input)
        {
            return await mediator.Send(new AddBookCommand { Input = input });
        }

        public async Task<bool> BuyBook([Service] IMediator mediator, int bookId)
        {
            return await mediator.Send(new BuyBookCommand { BookId = bookId });
        }

    }
}
