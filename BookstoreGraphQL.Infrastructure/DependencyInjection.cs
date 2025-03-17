using BookstoreGraphQL.Domain.Interfaces;
using BookstoreGraphQL.Infrastructure.Data;
using BookstoreGraphQL.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookstoreGraphQL.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookstoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IBookRepository, BookRepository>();

            // Register MediatR for Application Layer
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                Assembly.Load("BookstoreGraphQL.Application")
            ));

            return services;
        }
    }
}
