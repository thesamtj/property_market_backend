using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace property_market_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public int Username { get; set; }
        [Required]
        public int Password { get; set; }
    }
}
