using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Services
{
    public interface IUserService
    {
        public string GetCustomErrorsMessage(IdentityError error);
    }
}
