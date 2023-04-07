using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApplication.Models
{
    public class User
    {
        public User()
        {

        }
        public User (string fullName, string email, string login, string password, UserRole role)
        {
            FullName = fullName;
            Email = email;
            Login = login;
            Password = password;
            Role = role;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        
    }
}

namespace DeliveryApplication.Models
{
    public enum UserRole
    {
        Admin = 1,
        Customer = 2
    }
}