using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dookki_Web.Models.Map
{
    public class AdminViewModel
    {
        public int IDAccount { get; set; } // ID của tài khoản
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}