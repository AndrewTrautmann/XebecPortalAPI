using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XebecAPI.DTOs
{
    public class AppUserTwoDTO
    {

        public int Id { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Key { get; set; }
        public bool Registered { get; set; } = false;
        public int LinkVisits { get; set; }

        //used for registration
        public AppUserTwoDTO(string email, string PasswordHash, string role, string name, string surname)
        {
            Email = email;
            this.PasswordHash = PasswordHash;
            Role = role;
            Name = name;
            Surname = surname;
            LinkVisits = 0;
        }
        public AppUserTwoDTO(string email, string PasswordHash)
        {
            Email = email;
            this.PasswordHash = PasswordHash;
        }

        public AppUserTwoDTO()
        {

        }


    }
}
