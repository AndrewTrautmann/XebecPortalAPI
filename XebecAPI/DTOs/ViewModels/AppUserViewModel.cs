﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecAPI.DTOs.ViewModels
{
    public class AppUserViewModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Key { get; set; }
        public bool Registered { get; set; } = true;
        public DateTime TimeRegistered { get; set; }
        public int LinkVisits { get; set; }
        public DateTime TimeLoggedIn { get; set; }
        public DateTime TimeLoggedOut { get; set; }
    }
}
