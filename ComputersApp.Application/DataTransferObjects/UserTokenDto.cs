using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Application.DataTransferObjects
{
    public class UserTokenDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public TokenDto Token { get; set; }
    }
}
