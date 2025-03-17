using Microsoft.AspNetCore.Identity;

namespace BookstoreGraphQL.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
