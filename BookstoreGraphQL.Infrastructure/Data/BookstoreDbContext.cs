using BookstoreGraphQL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookstoreGraphQL.Infrastructure.Data
{
    public class BookstoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "J.K. Rowling" },
                new Author { Id = 2, Name = "George R.R. Martin" }
            );
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
               new IdentityRole { Name = "User", NormalizedName = "USER" }
           );
        }
    }
}
