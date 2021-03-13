using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace property_market_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Username { get; set; }
        public int Password { get; set; }
    }
}
