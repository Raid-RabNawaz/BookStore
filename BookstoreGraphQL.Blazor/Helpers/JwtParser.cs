using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookstoreGraphQL.Blazor.Helpers
{
    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            return jsonToken?.Claims ?? Enumerable.Empty<Claim>();
        }
    }
}
