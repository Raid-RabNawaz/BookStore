using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookstoreGraphQL.Infrastructure;
using BookstoreGraphQL.Infrastructure.Data;
using BookstoreGraphQL.Domain.Entities; 
using Serilog;
using BookstoreGraphQL.API.GraphQL.Queries;
using BookstoreGraphQL.API.GraphQL.Mutations;
using System.Reflection;
using BookstoreGraphQL.Application.DTOs;
using BookstoreGraphQL.Application.Services;
using BookstoreGraphQL.Domain.Interfaces;
using BookstoreGraphQL.Application.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Configure Identity & Authentication
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BookstoreDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy => policy.WithOrigins("http://localhost:5030", "https://localhost:7109/", "http://localhost:7109/")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddValidatorsFromAssemblyContaining<BookInputValidator>();
builder.Services.AddFluentValidationAutoValidation();
// Add GraphQL
builder.Services.AddGraphQLServer()
    .AddQueryType<BookQueries>()
    .AddMutationType<BookMutations>()
    .BindRuntimeType<BookDto>()
    .BindRuntimeType<BookInput>()
    .BindRuntimeType<AuthorDto>()
    .BindRuntimeType<AuthorInput>(); 

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
}
app.UseRouting();

app.UseCors("AllowBlazorClient");
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();