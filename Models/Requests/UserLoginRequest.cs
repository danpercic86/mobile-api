using System.ComponentModel.DataAnnotations;

namespace itec_mobile_api_final.Models.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}