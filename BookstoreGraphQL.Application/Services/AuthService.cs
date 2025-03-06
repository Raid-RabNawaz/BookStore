using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using BookstoreGraphQL.Domain.Entities;
using BookstoreGraphQL.Domain.Interfaces;
using BookstoreGraphQL.Application.DTOs;
using FluentValidation;

namespace BookstoreGraphQL.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IValidator<RegisterUserDto> _registerValidator;
        private readonly IValidator<LoginUserDto> _loginValidator;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IValidator<RegisterUserDto> registerValidator, IValidator<LoginUserDto> loginValidator)
        {
            _userManager = userManager;
            _configuration = configuration;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        public async Task<string> RegisterUser(string fullName, string email, string password, string role)
        {
            var model = new RegisterUserDto{
                FullName = fullName,
                Email = email,
                Password = password,
                Role = role
            };
            var validationResult = await _registerValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = new ApplicationUser { FullName = model.FullName, Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return null;

            await _userManager.AddToRoleAsync(user, model.Role);
            return GenerateJwtToken(user);
        }

        public async Task<string> LoginUser(string email, string password)
        {
            var model = new LoginUserDto { Email = email, Password = password };
            var validationResult = await _loginValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                return null;

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
