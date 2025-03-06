using Blazored.LocalStorage;
using BookstoreGraphQL.Blazor.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BookstoreGraphQL.Blazor.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public async Task NotifyUserAuthentication(string token)
        {
            await _localStorage.SetItemAsync("authToken", token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task NotifyUserLogout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
