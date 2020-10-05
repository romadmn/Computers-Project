using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Application.DataTransferObjects
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
