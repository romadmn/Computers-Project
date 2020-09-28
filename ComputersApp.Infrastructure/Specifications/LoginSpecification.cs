using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ComputersApp.Infrastructure.Specifications
{
    public class LoginSpecification : BaseSpecification<User>
    {
        public LoginSpecification(string email, string password) : base(p => p.Email == email && p.Password == password)
        {

        }
    }
}
