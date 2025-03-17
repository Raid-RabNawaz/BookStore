using BookstoreGraphQL.Domain.Entities;

namespace BookstoreGraphQL.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task<bool> BuyBookAsync(int bookId);
    }
}
