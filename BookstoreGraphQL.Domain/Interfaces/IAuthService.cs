using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreGraphQL.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUser(string fullName, string email, string password, string role);
        Task<string> LoginUser(string email, string password);
    }

}
