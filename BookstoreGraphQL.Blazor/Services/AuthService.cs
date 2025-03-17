using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace BookstoreGraphQL.Blazor.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> Register(string fullName, string email, string password, string role)
        {
            var query = new
            {
                query = @"
                mutation {
                    registerUser(fullName: """ + fullName + @""", email: """ + email + @""", password: """ + password + @""", role: """ + role + @""")
                }"
            };

            var response = await _http.PostAsJsonAsync("https://localhost:7292/graphql", query);
            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

            return jsonResponse.TryGetProperty("data", out var data) &&
                   data.TryGetProperty("registerUser", out _);
        }

        public async Task<bool> Login(string email, string password)
        {
            var query = new
            {
                query = @"
                mutation {
                    loginUser(email: """ + email + @""", password: """ + password + @""")
                }"
            };

            var response = await _http.PostAsJsonAsync("https://localhost:7292/graphql", query);
            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (jsonResponse.TryGetProperty("data", out var data) &&
                data.TryGetProperty("loginUser", out var tokenElement))
            {
                var token = tokenElement.GetString();
                await _localStorage.SetItemAsync("authToken", token);
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);
                return true;
            }

            return false;
        }

        public async Task<string> GetUserRole()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(token))
            {
                return "User"; // Default role
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var roleClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            return roleClaim?.Value ?? "User";
        }

        public async Task Logout()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (token != null)
            {
                var query = new
                {
                    query = @"
            mutation {
                logoutUser(token: """ + token + @""")
            }"
                };

                await _http.PostAsJsonAsync("https://localhost:7292/graphql", query);
            }

            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

    }
}
