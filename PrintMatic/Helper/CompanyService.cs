using Microsoft.AspNetCore.Identity;
using PrintMatic.Core.Entities.Identity;

namespace PrintMatic.Helper
{
    public class CompanyService
    {
        private readonly UserManager<AppUser> _userManager;

        public CompanyService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetCompanyLocationAsync(string traderId)
        {
            var trader = await _userManager.FindByIdAsync(traderId.ToString());
            return trader?.Location ?? "Unknown";
        }

        public async Task<string> GetCompanyNameAsync(string traderId)
        {
            var trader = await _userManager.FindByIdAsync(traderId.ToString());
            return trader?.UserName ?? "Unknown";
        }
    }

}
