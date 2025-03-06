using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BookstoreGraphQL.Blazor;
using Blazored.LocalStorage;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Components.Authorization;
using BookstoreGraphQL.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7292/graphql";

builder.Services.AddSingleton(_ => new GraphQLHttpClient(apiBaseUrl, new NewtonsoftJsonSerializer()));

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<BookService>();

builder.Services.AddScoped<ToastService>();


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();