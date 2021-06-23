using property_market_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace property_market_backend.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string userName, string password);
        void Register(string userName, string password);
        Task<bool> UserAlreadyExists(string userName);
    }
}
