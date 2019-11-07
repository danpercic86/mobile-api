using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace itec_mobile_api_final.Extensions
{
    public static class HttpContextExtensions
    { 
        public static async Task<IdentityUser> GetCurrentUserAsync(this HttpContext http,
            UserManager<IdentityUser> userManager)
        {
            var userId = http.GetCurrentUserId();
            if (userId is null)
                return null;
            
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }

        public static string GetCurrentUserId(this HttpContext http)
        {
            var userClaim = http.User.FindFirst("id");
            return userClaim?.Value;
        }
    }
}