using BookstoreGraphQL.Domain.Entities;
using BookstoreGraphQL.Domain.Interfaces;
using BookstoreGraphQL.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookstoreGraphQL.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookstoreDbContext _context;

        public BookRepository(BookstoreDbContext context) => _context = context;

        public async Task<IEnumerable<Book>> GetBooksAsync() => await _context.Books.Include(b => b.Author).ToListAsync();

        public async Task<Book> GetBookByIdAsync(int id) => await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> BuyBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.Stock <= 0) return false;

            book.Stock -= 1;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
