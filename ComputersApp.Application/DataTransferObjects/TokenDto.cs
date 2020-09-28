using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Application.DataTransferObjects
{
    public class TokenDto
    {
        public string JWT { get; set; }
        public string RefreshToken { get; set; }
    }
}
