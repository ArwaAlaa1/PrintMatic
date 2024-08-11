using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Identity
{
	public class AppUser:IdentityUser
	{
        public  string  DisplayName{ get; set; }
        public Address? Address { get; set; }
        public string? Location { get; set; }
        public string? Photo { get; set; }
        public bool IsCompany { get; set; }
    }
}
