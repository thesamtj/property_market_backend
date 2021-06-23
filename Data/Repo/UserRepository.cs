using Microsoft.EntityFrameworkCore;
using property_market_backend.Interfaces;
using property_market_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace property_market_backend.Data.Repo
{    
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dc;

        public UserRepository(DataContext dc)
        {
            this.dc = dc;
        }

        public async Task<User> Authenticate(string userName, string password)
        {
            return await dc.Users.FirstOrDefaultAsync(x => x.Username == userName /*&& x.Password == password*/);
        }

        public void Register(string userName, string password)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            User user = new User();
            user.Username = userName;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            dc.Users.Add(user);
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await dc.Users.AnyAsync(x => x.Username == userName);
        }
    }
}
