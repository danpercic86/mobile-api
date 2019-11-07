using System.Threading.Tasks;
using itec_mobile_api_final.Entities;
using itec_mobile_api_final.Models;
using itec_mobile_api_final.Models.Requests;

namespace itec_mobile_api_final.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> UpdateAsync(User user, UserUpdateRequest request);
    }
}