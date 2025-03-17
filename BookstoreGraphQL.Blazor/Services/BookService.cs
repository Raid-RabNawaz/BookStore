using GraphQL.Client.Http;
using GraphQL;
using BookstoreGraphQL.Application.DTOs;
using System.Text.Json;

namespace BookstoreGraphQL.Blazor.Services
{
    public class BookService
    {
        private readonly GraphQLHttpClient _client;

        public BookService(GraphQLHttpClient client)
        {
            _client = client;
        }

        public async Task<List<BookDto>> GetBooks()
        {
            var query = new GraphQLRequest
            {
                Query = "{ books { id title authorName stock price } }"
            };

            var response = await _client.SendQueryAsync<dynamic>(query);
            return response.Data.books.ToObject<List<BookDto>>();
        }

        public async Task<bool> BuyBook(int bookId)
        {
            var mutation = new GraphQLRequest
            {
                Query = $"mutation {{ buyBook(bookId: {bookId}) }}"
            };

            var response = await _client.SendMutationAsync<dynamic>(mutation);
            return response.Data.buyBook;
        }

        public async Task<BookDto> AddBook(string title, string authorName, int stock, decimal price, int publishedYear, string description)
        {
            JsonElement bookElement = new JsonElement();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation ($title: String!, $authorName: String!, $stock: Int!, $price: Decimal!, $publishedYear: Int!, $description: String!) {
                    addBook(title: $title, authorName: $authorName, stock: $stock, price: $price, publishedYear: $publishedYear, description: $description) {
                        id title authorName stock price publishedYear description
                    }
                }",
                Variables = new
                {
                    title,
                    authorName,
                    stock,
                    price,
                    publishedYear,
                    description
                }
            };

            var response = await _client.SendMutationAsync<dynamic>(mutation);

            if (response.Data != null && response.Data.TryGetProperty("addBook", out bookElement))
            {
                return bookElement.Deserialize<BookDto>();
            }

            return null;
        }

    }
}
