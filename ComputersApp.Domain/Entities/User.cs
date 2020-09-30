using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Domain.Entities
{
    public class User : IEntityBase
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
