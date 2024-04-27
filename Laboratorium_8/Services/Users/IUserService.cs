using Laboratorium_8.Entities;
using Laboratorium_8.Model;

namespace Laboratorium_8.Services.Users
{
    public interface IUserService
    {
        AuthenticationResponse
Authenticate(AuthenticationRequest request);
        IEnumerable<User> GetUsers();
        User GetByUsername(string username);
        User GetById(int id);

        int GetUsersCount();
        int GetPrime();
    }
}
