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

        public IEnumerable<Address>? Addresses { get; set; }=null;
       
       
        public string? Photo { get; set; }
        public string? FilePath { get; set; }

        public bool? IsCompany { get; set; }
    }
}
