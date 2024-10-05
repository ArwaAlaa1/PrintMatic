using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Services
{
    public class TokenService : ITokenService
    { 
        public IConfiguration Configuration { get; }
        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       

        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
        {
            #region Pivate Claims

            var authClaims = new List<Claim>()
            {
				new Claim(ClaimTypes.NameIdentifier, user.Id), // User ID claim
                new Claim(ClaimTypes.Email, user.Email), // Email claim
               // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim(ClaimTypes.Name, user.UserName),
             
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            #endregion

            #region Secret Key

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]));

            #endregion

            #region Token Object & Registered Claims

            var token = new JwtSecurityToken(

              issuer: Configuration["JWT:Issuer"],
              audience: Configuration["JWT:Audience"],
              expires: DateTime.UtcNow.AddDays(double.Parse(Configuration["JWT:DurationExpire"])),
              claims: authClaims,
              signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
              );

            #endregion

            //Token itself
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
