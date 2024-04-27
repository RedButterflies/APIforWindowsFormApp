using Laboratorium_8.Entities;
using Laboratorium_8.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Laboratorium_8.Services.Users
{
    public class UserServiceImpl : IUserService
    {
        private static List<User> users = new List<User>
         {
             new User{Id = 1, Username = "Andrzej", Password =
            "Andrzej", Roles = new List<Role>{ new Role { Role_ = "user"
            }, new Role { Role_ = "user" } } },
             new User{Id = 2, Username = "Piotrek", Password =
            "Piotrek", Roles = new List<Role>{ new Role { Role_ =
            "number" }, new Role { Role_ = "user" } } },
             new User{Id = 3, Username = "ola", Password =
            "haslo1234", Roles = new List<Role>{new Role { Role_ = "admin" } }
            }
         };
        private IConfiguration configuration;
        public UserServiceImpl(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            User user = GetByUsername(request.Username);
            if (user == null || user.Password != request.Password)
            {
                return null;
            }
            string token = generateJwtToken(user);
            return new AuthenticationResponse(user, token);
        }
        public User GetById(int id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }
        public User GetByUsername(string username)
        {
            return users.FirstOrDefault(x => x.Username == username);
        }
        public IEnumerable<User> GetUsers()
        {
            return users;
        }
        public int GetUsersCount()
        {
            return users.Count;
        } 
        public int GetPrime()
        {
            var rnd = new Random();
            var primes = new List<int> { 2, 3, 5, 7, 11, 13 };
            var randomPrime = primes[rnd.Next(primes.Count)];
            return randomPrime;
        }
        public string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:key"]);
            var claims = new List<Claim>();
            claims.Add(new Claim("id", user.Id.ToString()));
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role_));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
