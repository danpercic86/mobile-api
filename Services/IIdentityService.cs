using System.Threading.Tasks;
using itec_mobile_api_final.Models;

namespace itec_mobile_api_final.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}